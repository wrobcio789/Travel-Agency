package pg.rsww.redteam.payments.endpoints;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import pg.rsww.redteam.payments.models.MakePaymentRequest;
import pg.rsww.redteam.payments.services.InvalidPaymentException;
import pg.rsww.redteam.payments.services.PaymentService;

import javax.validation.Valid;

@RestController
@RequestMapping("/api/payments")
@AllArgsConstructor
public class PaymentRestEndpoint {

    private final PaymentService paymentService;

    @PostMapping("/pay")
    public ResponseEntity<String> pay(@Valid @RequestBody MakePaymentRequest request){
        try {
            paymentService.pay(request);
            return ResponseEntity.ok().build();
        } catch (InvalidPaymentException e) {
            return ResponseEntity.status(HttpStatus.UNPROCESSABLE_ENTITY).body(e.getMessage());
        }
    }
}
