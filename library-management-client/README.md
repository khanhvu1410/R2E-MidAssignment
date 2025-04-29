# Library Management System - React Frontend

A React frontend for NashTech Rookie 2 Engineer Mid Assignment.

## Features

- **Admin Dashboard**
  - Manage books (CRUD operations)

## Project Structure

```bash
src/
├── api/              # API service layer
├── components/       # Reusable UI components
├── constants/        # Application constants
├── models/           # Typescript interfaces
├── pages/            # Route-level components
├── routes/           # Routes configuration
├── setup/            # Third party setup
└── App.tsx           # Root component
```

## Prerequisites

- Node.js v16+
- npm v8+
- JSON Server for development

## Installation

```bash
# Clone the repository
git clone https://github.com/khanhvu1410/library-management-client.git
cd library-management-client

# Install dependencies
npm install
```

## Running the Application

```bash
# Start React development server
npm start

# In another terminal, start mock API server
npx json-server --watch db.json --port 8000
```

Access the app at: http://localhost:3000 <br> Mock API endpoint: http://localhost:8000
