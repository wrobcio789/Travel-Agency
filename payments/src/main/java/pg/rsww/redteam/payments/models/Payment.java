package pg.rsww.redteam.payments.models;

import lombok.Data;

import javax.persistence.*;

@Data
@Entity
@Table(name="payments")
public class Payment {
    @Id
    private String id;
    @Column(nullable = false)
    private String orderId;
    @Enumerated(EnumType.ORDINAL)
    private PaymentStatus status;
    @Embedded
    private Amount amount;
}
