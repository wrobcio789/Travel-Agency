package pg.rsww.redteam.payments.configuration;

import org.springframework.amqp.core.AmqpTemplate;
import org.springframework.amqp.core.Queue;
import org.springframework.amqp.rabbit.connection.ConnectionFactory;
import org.springframework.amqp.rabbit.core.RabbitTemplate;
import org.springframework.amqp.support.converter.Jackson2JsonMessageConverter;
import org.springframework.amqp.support.converter.MessageConverter;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class RabbitQueueConfiguration {

    public static final String CANCEL_RESERVATION_QUEUE = "cancel-reservation";
    public static final String CREATE_PAYMENT_QUEUE = "create-payment";

    @Bean
    public Queue cancelPaymentQueue() {
        return new Queue(CANCEL_RESERVATION_QUEUE);
    }

    @Bean
    public Queue createPaymentQueue() {
        return new Queue(CREATE_PAYMENT_QUEUE);
    }

    @Bean
    public MessageConverter messageConverter() {
        return new Jackson2JsonMessageConverter();
    }


    @Bean
    public AmqpTemplate template(ConnectionFactory connectionFactory){
        RabbitTemplate rabbitTemplate = new RabbitTemplate(connectionFactory);
        rabbitTemplate.setMessageConverter(messageConverter());

        return rabbitTemplate;
    }
}
