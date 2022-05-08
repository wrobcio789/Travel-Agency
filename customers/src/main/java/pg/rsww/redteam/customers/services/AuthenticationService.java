package pg.rsww.redteam.customers.services;

import lombok.AllArgsConstructor;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Service;
import pg.rsww.redteam.customers.models.AuthenticationRequest;

@Service
@AllArgsConstructor
public class AuthenticationService {

    private final AuthenticationManager authenticationManager;
    private final AuthenticationTokenManager authenticationTokenManager;

    public String generateSessionToken(AuthenticationRequest request) throws AuthenticationException {
        final Authentication authentication = new UsernamePasswordAuthenticationToken(request.getUsername(), request.getPassword());

        final UserDetails userDetails = (UserDetails) authenticationManager.authenticate(authentication).getPrincipal();

        return authenticationTokenManager.generateToken(userDetails);
    }
}
