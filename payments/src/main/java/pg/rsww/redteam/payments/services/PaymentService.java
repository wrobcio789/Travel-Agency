package pg.rsww.redteam.payments.services;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import pg.rsww.redteam.payments.models.CreatePaymentRequest;
import pg.rsww.redteam.payments.models.Payment;
import pg.rsww.redteam.payments.models.MakePaymentRequest;
import pg.rsww.redteam.payments.models.PaymentStatus;
import pg.rsww.redteam.payments.repositories.PaymentForOrderRepository;

@Service
@AllArgsConstructor
public class PaymentService {
    private final CardPaymentProvider cardPaymentProvider;
    private final PaymentForOrderRepository paymentRepository;

    public void pay(MakePaymentRequest request) throws InvalidPaymentException {
        final Payment payment = paymentRepository.findById(request.getPaymentId()).orElse(null);

        validatePayment(payment);
        makePayment(request, payment);
    }

    public Payment create(CreatePaymentRequest request) {
        final Payment payment = new Payment();

        payment.setStatus(PaymentStatus.CREATED);
        payment.setAmount(request.getAmount());
        payment.setOrderId(request.getOrderId());

        return paymentRepository.saveAndFlush(payment);
    }

    private void validatePayment(Payment payment) throws InvalidPaymentException {
        if(payment == null) {
            throw new InvalidPaymentException("Payment is not registered!");
        }

        if (payment.getStatus() != PaymentStatus.CREATED) {
            throw new InvalidPaymentException("Payment can no longer be processed");
        }
    }

    private void makePayment(MakePaymentRequest request, Payment payment) throws InvalidPaymentException {
        try {
            cardPaymentProvider.makePayment(request.getCardData(), payment.getAmount());
        } catch (CardPaymentException e) {
            throw new InvalidPaymentException(String.format("Could not charge card: %s", e.getMessage()));
        }

        payment.setStatus(PaymentStatus.SUCCESSFUL);
        paymentRepository.saveAndFlush(payment);
    }

}
