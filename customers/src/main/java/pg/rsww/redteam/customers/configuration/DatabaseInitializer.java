package pg.rsww.redteam.customers.configuration;

import lombok.AllArgsConstructor;
import org.springframework.boot.CommandLineRunner;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Component;
import pg.rsww.redteam.customers.models.AppUser;
import pg.rsww.redteam.customers.repositories.AppUserRepository;

@Component
@AllArgsConstructor
public class DatabaseInitializer implements CommandLineRunner {

    private final PasswordEncoder passwordEncoder;
    private final AppUserRepository appUserRepository;

    @Override
    public void run(String... args) throws Exception {
        appUserRepository.save(new AppUser("maciej", passwordEncoder.encode("1234")));
        appUserRepository.save(new AppUser("hubert", passwordEncoder.encode("abcd")));
        appUserRepository.save(new AppUser("student", passwordEncoder.encode("student")));
        appUserRepository.save(new AppUser("admin", passwordEncoder.encode("admin")));
    }
}