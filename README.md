# University Management Application

## Opis aplikacji

Aplikacja University Management to rozbudowany system zarządzania uczelnią, który obejmuje trzy główne warstwy prezentacji: RestApi, GraphQL oraz Razor Pages. Aplikacja jest zaprojektowana zgodnie z architekturą Clean Architecture, co umożliwia łatwe rozszerzanie i utrzymanie kodu.

### Warstwy aplikacji

1. **RestApi**: Zapewnia interfejs RESTful do zarządzania zasobami aplikacji.
2. **GraphQL**: Umożliwia bardziej elastyczne zapytania i manipulację danymi.
3. **Razor Pages**: Oferuje interfejs użytkownika do zarządzania administracyjnego.

### Technologie

- .NET 8.0
- Entity Framework Core
- AutoMapper
- ASP.NET Core Identity
- JWT Authentication
- GraphQL z użyciem HotChocolate
- Razor Pages

## Instrukcja uruchomienia testów

### Wymagane narzędzia

Przed uruchomieniem testów, upewnij się, że masz zainstalowane następujące narzędzia:

- **jq**: Narzędzie do przetwarzania JSON w linii poleceń. Można je zainstalować za pomocą menedżera pakietów dla systemu WSL (np. `apt` dla Ubuntu).

#### Instalacja jq

Dla systemu Ubuntu (WSL):

```sh
sudo apt-get update
sudo apt-get install jq
```

#### Uruchomienie testów GraphQL
1. Otwórz terminal WSL.

2. Przejdź do katalogu z plikiem graphql_tests.sh.

3. Wykonaj skrypt:

```sh
chmod +x graphql_tests.sh
./graphql_tests.sh
```

#### Uruchomienie testów RestApi
1. Otwórz terminal WSL.

2. Przejdź do katalogu z plikiem restapi_tests.sh.

3. Wykonaj skrypt:

```sh
chmod +x restapi_tests.sh
./restapi_tests.sh
```

### Instrukcja uruchomienia dwóch projektów jednocześnie (Razor Pages i RestApi)
#### Aby uruchomić jednocześnie projekty Razor Pages i RestApi, postępuj zgodnie z poniższymi krokami:

1. Otwórz Visual Studio.
2. Przejdź do opcji uruchamiania projektu:
3. Kliknij prawym przyciskiem myszy na rozwiązanie w Solution Explorer.
4. Wybierz Properties.
6. Wybierz opcję Multiple startup projects.
7. Ustaw Action na Start dla projektów University.RazorPages i University.RestApi.
8. Kliknij Apply, a następnie OK.
Teraz, gdy uruchomisz rozwiązanie, oba projekty powinny uruchomić się jednocześnie, umożliwiając pełną funkcjonalność aplikacji.

#### Uwaga:
Jeśli zmienisz projekty do uruchomienia w trybie pojedynczego projektu, będziesz musiał ponownie skonfigurować uruchamianie wielu projektów według powyższej instrukcji.
