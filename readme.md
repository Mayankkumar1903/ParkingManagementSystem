# Parking Management Application

This Parking Management Application is designed for managing parking spaces, allocating them to vehicles, and generating reports. The application provides separate interfaces for Booking Counter Agents and Parking Zone Assistants.

## Technologies Used

The project is developed using ASP.NET MVC. The solution is organized with a clean and modular object-oriented approach, ensuring code reusability, encapsulation, and maintainability.

## Setup Instructions

### Prerequisites

- Visual Studio (Version X or later)
- .NET Framework (Version Y or later)

### Installation Steps
1. Open the solution in Visual Studio.

2. Build the solution to restore dependencies:

   dotnet build

3. Configure the database connection string in the `web.config` and `app.config` file.

4. Start the application in Visual Studio.

5. Open a web browser and navigate to `http://localhost:44379` to access the Parking Management Application.

## Usage

### Sign-in

- Two types of users can sign in: Booking Counter Agent and Parking Zone Assistant.

### Initialize (Booking Counter Agent Only)

- Click on the "Initialize" button to set up the initial data.
- Add three parking zones: A, B, and C.
- Add 30 parking spaces: A01...A10, B01...B10, and C01...C10 and all
- Remove all transactional data.

### Dashboard

#### Parking Space Listing

- View a list of parking spaces sorted by title and filtered by parking zone.
- Displays parking space title, availability (Vacant: Green, Occupied: Gray), and vehicle registration number if occupied.
- The list auto-refreshes based on bookings or releases.

#### Book Parking Space (Booking Counter Agent Only)

- Enter the vehicle registration number to book a parking space.

#### Release Parking Space (Booking Counter Agent Only)

- Enter the vehicle registration number to release a parking space.

### Reports

#### Parking Zone Report

- View the parking zone report on the browser.
- Export the report to PDF.

## Entities / Attributes

### User

- user_id
- name
- email (used as a username for sign-in)
- password
- type (Booking Counter Agent, Parking Zone Assistant)

### Parking Zone

- parking_zone_id
- parking_zone_title

### Parking Space

- parking_space_id
- parking_space_title
- parking_zone_id

### Vehicle Parking

- vehicle_parking_id
- parking_zone_id
- parking_space_id
- booking_date_time
- release_date_time

