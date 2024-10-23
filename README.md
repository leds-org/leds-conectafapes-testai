# leds-conectafapes-testai

**leds-conectafapes-testai** is a tool designed to automatically generate BDD (Behavior-Driven Development) test scenarios in Gherkin format, leveraging AI for intelligent scenario generation. This project integrates with FastAPI and Docker to streamline the creation of `.feature` files for your test automation needs.

## Features

- **Automated BDD Scenario Generation**: Generate Gherkin test scenarios based on AI-driven logic.
- **FastAPI Integration**: Simple API to trigger feature generation and receive code in the response body.
- **Dockerized Setup**: Easily deploy and run the tool in any environment with Docker.
- **Customizable Directory Structure**: Save generated `.feature` files directly into designated folders within the container.

### Usage

You can send a request to the API to generate a BDD scenario in Gherkin format. The API will return the generated Gherkin code in the response body, and also create a `.feature` file inside the container.

#### Example Request

```bash
POST http://localhost:8000/gherkin
Content-Type: application/json

{
  "evento": "Your scenario description"
}
```

#### Example Response

```gherkin
Feature: Example Feature

  Scenario: Your scenario description
    Given some initial context
    When an action is performed
    Then an expected outcome occurs
```
