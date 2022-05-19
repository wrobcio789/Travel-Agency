package pg.rsww.redteam.customers.endpoints;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.web.bind.annotation.*;
import pg.rsww.redteam.customers.models.AuthenticationRequest;
import pg.rsww.redteam.customers.services.AuthenticationService;

import javax.validation.Valid;

@RestController
@RequestMapping("/api/customers")
@AllArgsConstructor
public class AuthenticationRestEndpoint {

    private final AuthenticationService authenticationService;

    @PostMapping("/login")
    public ResponseEntity<String> login(@Valid @RequestBody AuthenticationRequest authenticationRequest) {
        try {
            final String token = authenticationService.generateSessionToken(authenticationRequest);
            return ResponseEntity.ok(token);
        } catch (AuthenticationException e) {
            return ResponseEntity.status(HttpStatus.UNAUTHORIZED).build();
        }
    }

    @GetMapping("/hello")
    public ResponseEntity<String> hello() {
        final String username = (String) SecurityContextHolder.getContext().getAuthentication().getPrincipal();
        return ResponseEntity.ok(String.format("Hello %s", username));
    }
}
