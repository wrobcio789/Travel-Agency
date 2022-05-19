package pg.rsww.redteam.payments.services;

import lombok.AllArgsConstructor;
import org.springframework.amqp.rabbit.annotation.RabbitListener;
import org.springframework.stereotype.Component;
import pg.rsww.redteam.payments.configuration.RabbitQueueConfiguration;
import pg.rsww.redteam.payments.models.CreatePaymentRequest;
import pg.rsww.redteam.payments.models.Payment;

@Component
@AllArgsConstructor
public class PaymentServiceListener {
    private final PaymentService paymentService;

    @RabbitListener(queues = RabbitQueueConfiguration.CANCEL_RESERVATION_QUEUE)
    public void listenOnReservationCancel(String orderId) {
        paymentService.cancel(orderId);
    }

    @RabbitListener(queues = RabbitQueueConfiguration.CREATE_PAYMENT_QUEUE)
    public String listenOnPaymentCreation(CreatePaymentRequest request) {
        final Payment payment = paymentService.create(request);

        return payment.getId();
    }
}
