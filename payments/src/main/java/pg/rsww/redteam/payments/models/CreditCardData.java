package pg.rsww.redteam.payments.models;

import lombok.Data;
import org.springframework.lang.NonNull;

@Data
public class CreditCardData {
    @NonNull
    private final String creditCardNumber;
    private final int cvCode;
    @NonNull
    private final String expirationDate;
}
