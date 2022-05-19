package pg.rsww.redteam.payments.models;

import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.lang.NonNull;

@Data
@NoArgsConstructor
public class CreatePaymentRequest {
    @NonNull
    private String orderId;
    @NonNull
    private Amount amount;

}
