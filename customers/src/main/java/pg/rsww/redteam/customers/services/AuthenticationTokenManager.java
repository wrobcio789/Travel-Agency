package pg.rsww.redteam.customers.services;

import com.auth0.jwt.JWT;
import com.auth0.jwt.JWTVerifier;
import com.auth0.jwt.algorithms.Algorithm;
import com.auth0.jwt.exceptions.JWTVerificationException;
import com.auth0.jwt.interfaces.DecodedJWT;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Component;
import pg.rsww.redteam.customers.models.ValidatedToken;

import java.util.Date;

@Component
public class AuthenticationTokenManager {

    //TODO: create a way not to hardcode the secret
    private final Algorithm algorithm = Algorithm.HMAC256("secret");

    public String generateToken(UserDetails userDetails) {
        final String[] authorities = parseUserAuthorities(userDetails);

        return JWT.create()
                .withIssuedAt(new Date())
                .withSubject(userDetails.getUsername())
                .withArrayClaim("authorities", authorities)
                .sign(algorithm);
    }

    public ValidatedToken validate(String token) throws InvalidAuthenticationTokenException {
        final JWTVerifier jwtVerifier = JWT.require(algorithm).build();

        try {
            final DecodedJWT jwt = jwtVerifier.verify(token);
            return new ValidatedToken(
                    jwt.getSubject(),
                    jwt.getClaim("authorities").asArray(String.class)
            );
        } catch ( JWTVerificationException e) {
            throw new InvalidAuthenticationTokenException(e.getMessage());
        }

    }

    private String[] parseUserAuthorities(UserDetails userDetails){
        return userDetails.getAuthorities()
                .stream()
                .map(GrantedAuthority::getAuthority)
                .toArray(String[]::new);
    }

}
