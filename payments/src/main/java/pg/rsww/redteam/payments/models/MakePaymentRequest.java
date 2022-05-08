package pg.rsww.redteam.payments.models;

import lombok.Data;
import org.springframework.lang.NonNull;

import javax.validation.Valid;

@Data
public class MakePaymentRequest {
    @NonNull
    private final String paymentId;
    @Valid
    private final CreditCardData cardData;
}
