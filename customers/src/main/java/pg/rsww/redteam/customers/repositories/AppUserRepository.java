package pg.rsww.redteam.customers.repositories;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;
import pg.rsww.redteam.customers.models.AppUser;

import java.util.Optional;


@Repository
@Transactional
public interface AppUserRepository extends JpaRepository<AppUser, String> {
    Optional<AppUser> findByUsername(String username);
}
