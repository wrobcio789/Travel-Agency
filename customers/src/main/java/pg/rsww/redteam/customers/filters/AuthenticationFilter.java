package pg.rsww.redteam.customers.filters;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.lang.NonNull;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;
import pg.rsww.redteam.customers.models.ValidatedToken;
import pg.rsww.redteam.customers.services.AuthenticationTokenManager;
import pg.rsww.redteam.customers.services.InvalidAuthenticationTokenException;

import javax.servlet.FilterChain;
import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Arrays;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Component
@AllArgsConstructor
public class AuthenticationFilter extends OncePerRequestFilter {
    private final String AUTHORIZATION_HEADER = "Authorization";

    private final AuthenticationTokenManager authenticationTokenManager;


    @Override
    protected boolean shouldNotFilter(HttpServletRequest request) throws ServletException {
        final String path = request.getRequestURI();
        return "/api/customers/login".equals(path);
    }

    @Override
    protected void doFilterInternal(@NonNull HttpServletRequest request, @NonNull HttpServletResponse response, @NonNull FilterChain filterChain) throws IOException, ServletException {
        final Optional<String> bearerToken = getBearerAuthenticationToken(request);

        if (bearerToken.isPresent()) {
            doFilterInternalWithToken(bearerToken.get(), request, response, filterChain);
        } else {
            onAuthenticationFail(response);
        }

    }

    private void doFilterInternalWithToken(String token, HttpServletRequest request, HttpServletResponse response, FilterChain filterChain) throws IOException, ServletException {

        try {
            final ValidatedToken validatedToken  = authenticationTokenManager.validate(token);
            final Authentication authentication = createAuthentication(validatedToken);

            SecurityContextHolder.getContext().setAuthentication(authentication);
            filterChain.doFilter(request, response);
        } catch (InvalidAuthenticationTokenException e) {
            onAuthenticationFail(response);
        }
    }

    private void onAuthenticationFail(HttpServletResponse response) {
        SecurityContextHolder.clearContext();
        response.setStatus(HttpStatus.UNAUTHORIZED.value());
    }

    private Optional<String> getBearerAuthenticationToken(HttpServletRequest request) {
        String authorizationHeader = request.getHeader(AUTHORIZATION_HEADER);

        if (authorizationHeader != null && authorizationHeader.startsWith("Bearer ")) {
            return Optional.of(authorizationHeader.substring(7));
        }

        return Optional.empty();
    }

    private Authentication createAuthentication(ValidatedToken token) {
        List<GrantedAuthority> authorities = Arrays.stream(token.getAuthorities())
                .map(SimpleGrantedAuthority::new)
                .collect(Collectors.toList());

        return new UsernamePasswordAuthenticationToken(token.getSubject(), null, authorities);
    }
}
