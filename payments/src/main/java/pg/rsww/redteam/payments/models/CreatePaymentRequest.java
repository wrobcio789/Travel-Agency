package pg.rsww.redteam.payments.models;

import lombok.Data;
import org.springframework.lang.NonNull;

@Data
public class CreatePaymentRequest {
    @NonNull
    private String orderId;
    @NonNull
    private Amount amount;

}
