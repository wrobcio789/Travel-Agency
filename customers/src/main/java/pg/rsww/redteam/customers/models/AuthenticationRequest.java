package pg.rsww.redteam.customers.models;

import lombok.Data;
import org.springframework.lang.NonNull;

@Data
public class AuthenticationRequest {
    @NonNull
    private final String username;
    @NonNull
    private final String password;
}
