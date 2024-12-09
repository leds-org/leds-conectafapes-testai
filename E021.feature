Feature: Calendar Year Selection and Milestone Display

  Scenario Outline: Successful Year Selection and Milestone Display
    Given the current year is <current_year>
    When the user selects the year <selected_year>
    Then the system displays the calendar for the year <selected_year>
    And the system displays milestone M1: Deadline for Scholarship Requests for each month of <selected_year>
    And the system displays milestone M2: Expected Normal Payslip Generation Date for each month of <selected_year>
    And the system displays milestone M3: Normal Payslip Payment Date for each month of <selected_year>

    Examples:
      | current_year | selected_year |
      | 2024         | 2024         |
      | 2024         | 2023         |
      | 2024         | 2025         |
      | 2023         | 2023         |
      | 2025         | 2025         |
      | 2022         | 2024         |
      | 2023         | 2022         |
      | 2025         | 2026         |


  Scenario Outline: Error Handling for Invalid Year Selection
    Given the current year is <current_year>
    When the user selects the year <selected_year>
    Then the system displays an error message "<error_message>"
    And the system does not display the calendar

    Examples:
      | current_year | selected_year | error_message                       |
      | 2024         | abc           | Invalid year format. Please enter a valid year (YYYY). |
      | 2024         | 1800          | Year out of range. Please select a year within the allowed range. |
      | 2024         | 2024.5        | Invalid year format. Please enter a valid year (YYYY). |
      | 2024         | -2024         | Invalid year. Year must be positive. |
      | 2024         | 99999999999   | Year out of range. Please select a year within the allowed range. |
      | 2024         | 20250         | Year out of range. Please select a year within the allowed range. |
      | 2024         | 202a          | Invalid year format. Please enter a valid year (YYYY). |


  Scenario Outline: Missing Milestone Data Handling
    Given the year <selected_year> is selected
    When the system attempts to retrieve milestone data for the month of <month>
    And milestone data for <month> is missing for milestone <milestone>
    Then the system displays a message indicating missing data for milestone <milestone> in <month> of <selected_year>

    Examples:
      | selected_year | month       | milestone |
      | 2024         | January     | M1         |
      | 2024         | February    | M2         |
      | 2024         | March       | M3         |
      | 2023         | December    | M1         |
      | 2025         | April       | M3         |


  Scenario: Handling Unexpected Errors During Data Retrieval
    Given the year <selected_year> is selected
    When the system attempts to retrieve milestone data
    And an unexpected error occurs during data retrieval
    Then the system displays a generic error message indicating failure to retrieve milestone data

  Scenario: No Milestones Defined for Selected Year
    Given the year <selected_year> is selected
    When the system attempts to retrieve milestone data
    And no milestones are defined for <selected_year>
    Then the system displays a message indicating no milestones are defined for <selected_year>

    Examples:
      | selected_year |
      | 2000         |
      | 2100         |

  Scenario: Empty Year Selection
    Given the current year is 2024
    When the user selects an empty year field
    Then the system displays an error message "Please select a year"
    And the system does not display the calendar

  Scenario: Selecting Current Year
    Given the current year is 2024
    When the user selects the year 2024
    Then the system displays the calendar for the year 2024
    And a message confirms that the current year is selected.