package pg.rsww.redteam.payments.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import pg.rsww.redteam.payments.models.Payment;

import java.util.Optional;

@Repository
@Transactional
public interface PaymentForOrderRepository extends JpaRepository<Payment, String> {
    Optional<Payment> findByOrderId(String orderId);
}
