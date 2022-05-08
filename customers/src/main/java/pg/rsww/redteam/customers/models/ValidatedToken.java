package pg.rsww.redteam.customers.models;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class ValidatedToken {
    private final String subject;
    private final String[] authorities;
}
