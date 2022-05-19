package pg.rsww.redteam.payments.models;

import lombok.Data;

import javax.persistence.Column;
import javax.persistence.Embeddable;
import java.math.BigDecimal;

@Data
@Embeddable
public class Amount {
    @Column(columnDefinition = "DECIMAL(7,2)")
    private BigDecimal value;
    private String currency;
}
