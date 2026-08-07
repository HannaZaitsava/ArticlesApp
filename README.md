# Articles Management System

A RESTful API for managing articles, built with ASP.NET Core. This repository is configured for an one-command deployment using Docker and Docker Compose, including production-ready tools for database management, caching, logging, and health monitoring.

## Project Overview & Purpose
This project was developed for educational purposes to study, practice, and implement modern architectural patterns, tools, and best practices in web development.

The project is **currently under active development** and is not a final product. It serves as a sandbox for mastering scalable, maintainable, and resilient backend systems.


## Current Tech Stack & Approaches:
- Frontend: not ready yet
- Backend: .NET 10 / ASP.NET Core Web API. Formed around **Clean Architecture**
- Authentication: ASP .NET Core Identity
- Database: PostgreSQL 18
- Caching: **.NET HybridCache** (L1 In-Memory + L2 Redis 8.0) to optimize query performance and reduce primary database load.
- Logging: Serilog & Seq
- Containerization: The application and its infrastructure dependencies (Database, Redis, Seq) are fully containerized using **Docker** and **Docker Compose** for local development.


## Quick Start with Docker (Recommended)

### Prerequisites
- Docker and Docker Compose
Make sure you have [Docker Desktop](https://docker.com) installed and running on your machine.
- .NET SDK 10.0
- Windows 11 OS

### Installation & Launch Steps

1. **Clone the repository:**
   ```bash
   git clone https://github.com/HannaZaitsava/ArticlesApp.git
   cd project-root-directory
   ```

2. **Set up Environment Variables:**
   Create a `.env` file in the root directory (on the same level as `docker-compose.yml`) using the provided `.env-example` template:
   ```bash
   cp .env-example .env
   ```
   *Note: Ensure all passwords, usernames, and ports match your local preferences before starting.*

3. **Build and run the containers:**
   ```bash
   docker-compose up -d --build
   ```

4. Ensure docker containers are up and running correctly

5. **Verify the API status:**
   Open `https://localhost:8081` in your browser.

6. **Access Swagger UI:**
   The Swagger documentation is now available at:
   `https://localhost:8081/swagger/index.html`


### Infrastructure & Ecosystem Tools

Once the containers are successfully running, you can access the following integrated services using the ports defined in your configuration:

### Health Dashboard
* **URL:** `https://localhost:8081/health-dashboard`
* **Description:** Provides a real-time visual dashboard monitoring the status of the Web API, PostgreSQL database, and Redis cache.

### pgAdmin (PostgreSQL Management GUI)
* **URL:** `http://localhost:5051`
* **How to connect:**
  1. Log in using the credentials specified in your `.env` file (e.g., `admin@example.com` / `admin_password`)
  2. The pre-configured server connection file (`pg-admin-servers.json`) will automatically register the database.
  3. Expand the server tree and enter your database password to start executing queries.

### RedisInsight (Cache Management UI)
* **URL:** `http://localhost:5540`
* **How to connect:**
  1. Click **Add Redis database**.
  2. Set the **Host** to `redis_cache` (the internal Docker service name) and the **Port** to `6379`.
  3. Submit to visually inspect keys, manage distributed cache state, and track TTLs.

### Seq (Structured Logs UI)
* **URL:** `http://localhost:5341`
* **No additional setup required.
* **Description:** Aggregates structured logs sent by Serilog from your Web API application. You can write SQL-like queries to filter, view, and inspect app errors or performance metrics in real time.


## Local start

If you want to start the Web API project directly from your IDE, you still need the infrastructure (Redis) running. Follow these steps for the optimal development workflow:

### Prerequisites
* [.NET 10 SDK](https://microsoft.com) installed on your system.
* Visual Studio 2022 (or IDE of your choise).

### **1. Choose Your Infrastructure (Redis) Setup**

You can run the application locally using one of the two options below, depending on whether you want to test the full L2 distributed caching layer or just run the bare minimum.

The application utilizes **.NET HybridCache** (L1 In-Memory + L2 Redis).
Thanks to its resilient architecture, the application will not crash if the L2 infrastructure is missing; it will gracefully fall back to L1-only mode.

#### Option A: Hybrid Cache Mode (Recommended)
*Use this option to test the full caching lifecycle (L1 + L2 Redis).*

#### Redis setup using Docker
   1. **Spin up the required infrastructure only:**
    Run only Redis:
    ```bash
    docker run -d --name my_local_redis -p 6380:6379 redis:8.0-alpine
    ```
   2. **Review your Local Configuration:**
    Ensure your `ArticlesAPI/appsettings.json` file points to `localhost` and maps to the correct external port (e.g. 6380):
    ```json
    {
      "CacheSettings": {
        "RedisUrl": "localhost:6380,connectTimeout=5000,syncTimeout=5000,abortConnect=false"
      }
    }
    ```

#### Option B: Standalone Memory Mode (Zero Docker Setup)
*Use this option if you want to run the project instantly without launching Docker at all.*

  1. **Keep Docker completely turned off.**

  2. **Automatic Resilient Fallback:**
   You do not need to change any configuration keys. When you launch the application, `HybridCache` will notice that the L2 Redis instance at `localhost:6380` is unreachable. It will seamlessly intercept the connection error and operate strictly on a **L1 In-Memory Cache ➡️ Database** workflow.

  3. **Or you can install Redis natively on your OS:**

  * **Windows:** Install Redis via WSL2 (recommended by Redis) or use the archived native MSI/zip ports (e.g., [Memurai](https://memurai.com) or [tporadowski/redis](https://github.com)).
  * **macOS:** Install via Homebrew: `brew install redis` and start it with `brew services start redis`.
  * **Linux:** Install via your package manager: `sudo apt install redis-server`.

Once installed natively, update your `appsettings.Development.json` to use the standard default port: `localhost:6379`.


### **2. Run the Project:**
   * Open the `.sln` file in Visual Studio.
   * Set `ArticlesAPI` as the **Startup Project**.
   * Select the **ArticlesAPI** launch profile (not the Docker profile) and press **Ctrl+F5/F5**.
   * The app will start locally.
   * The browser will automatically load the local URL (e.g., `http://localhost:5000/swagger`).
   * Health Dashboard URL: `http://localhost:5000/health-dashboard`
   * Seq Web UI URL: `http://localhost:5341`



## 🔑 Authentication & Swagger Guide

At this stage, the project utilizes the built-in **ASP.NET Core Identity** system. To interact with protected endpoints (e.g., creating, editing, deleting or publishing articles), you can authenticate directly through the **Swagger UI**:

### Step 1: Login (Acquiring the Access Token)
1. Open the Swagger UI page in your browser.
2. Find the authentication endpoint (a `POST` request like `/login`).
3. Click **Try it out** and provide your credentials in the request body:
```json
{
  "email": "user@example.com",
  "password": "your_password"
}
```
4. Execute the request. The server will return a response containing an `accessToken` (a string encoded in **Base64** format). Copy this token string.

### Step 2: Authorizing in Swagger
1. Scroll to the very top of the Swagger UI page and click the lock icon button labeled **Authorize**.
2. In the input field, type `Bearer`, then **add a single space**, and paste your copied Base64 token.
   * *Example:* `Bearer CfDJ8J...`
3. Click **Authorize** and close the modal window.

All secure endpoints will now automatically include this token in their headers, allowing you to test the API seamlessly.


## Users credentials
- Admin:
  - Username: admin@gmail.com
  - Password: Admin111#
- Regular user
  - Username: member@gmail.com
  - Password: Member111#


## Contact
* **Author:** Hanna Zaitsava
* **GitHub:** https://github.com/HannaZaitsava/
* **email:** hanna.zaitsava.work@gmail.com


## Future Roadmapp

I plan to continuously expand the functionality and evolution of this system by implementing the following phases:

- **Identity & Access Management:** Migrate from built-in ASP.NET Core Identity to a robust, external production-ready identity provider using **Keycloak**.
- **Asynchronous Processing:** Implement Domain Events and set up background processing via **Background Workers**.
- **Frontend Application:** Build a dedicated client-side application (Frontend UI) to interact with the API.
- **Scalability:** Refactor the codebase to transition from a monolithic structure toward a **Microservices Architecture**.


,la,la,la