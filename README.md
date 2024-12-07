# Vibez Social Media Project

## Introduction

Welcome to **Vibez**, a social media platform designed and developed by Patrick Orjieh and Robert Nugent who aimed to bring people closer through real-time communication. This project showcases our combined efforts in building a fully functional platform that includes posts, messaging, calls, and more.

With **over 35 APIs**, we’ve covered everything from user management to real-time calls, using **SignalR** for instant messaging and **WebRTC** for voice and video communication (Partially working).

### Access the Project:

- **Frontend**: [Vibez Frontend](https://precious-entremet-b0f82e.netlify.app/)
- **Backend API**: [Vibez API](https://vibez-web-service-g8gzbmfvdnc2hahw.northeurope-01.azurewebsites.net)

---

## Features

Here’s what Vibez offers:

### User Management

- Users can sign up, log in, and personalize their profiles with profile pictures and bios.

### Posts

- Share posts with text and images.
- Explore activity feeds showcasing posts from friends or the community.

### Comments

- Engage in conversations by commenting on posts.
- Edit or delete your own comments when needed.

### Likes

- Like or unlike posts with real-time updates to the like count.

### Messaging

- **SignalR** ensures instant message delivery for real-time conversations.

### Calls

- **WebRTC** enables voice calls (Partially working).

### Friendships

- Send, accept, or decline friend requests and manage your friend list.

### Notifications

- Real-time notifications for events like likes, comments, and friend requests.

---

## APIs

The backend includes **35+ APIs** covering:

- **Users**: Account creation, authentication, and profile management.
- **Posts**: Create, update, and delete posts.
- **Comments**: Manage comments on posts.
- **Likes**: Add or remove likes on posts.
- **Messages**: Send and retrieve messages.
- **Calls**: Handle real-time voice and video calls.
- **Friendships**: Manage friend requests and statuses.
- **Notifications**: Fetch and mark notifications.

We tested these APIs with **Postman**, ensuring reliability across all features.

---

## Real-Time Features

Vibez’s real-time capabilities make it stand out:

- **SignalR**: Powering instant messaging for seamless communication.
- **WebRTC**: Enabling smooth and high-quality peer-to-peer voice calls.

---

## Architecture and Design Patterns

We prioritized clean, maintainable, and scalable code throughout the project. Here’s how we structured our work:

### Patterns and Principles

- **Repository Pattern**: Ensures a clear separation of database logic gotten from this [article](https://medium.com/@pererikbergman/repository-design-pattern-e28c0f3e4a30).
- **Unit of Work**: Manages transactions for database consistency gotten from microsoft [docs](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application).
- **Dependency Injection (DI)**: Promotes modular and testable code.
- **SOLID Principles**: Made the system more robust and adaptable.

---

## Testing

We developed **32 unit tests** using **xUnit** to ensure our system’s reliability. These tests covered services, repositories, and controllers. Additionally, we tested the APIs thoroughly using **Postman** to validate their functionality under various scenarios.

---

## Screenshots

Here are some screenshots of the Vibez platform:

### Messaging

![Messaging](https://github.com/user-attachments/assets/5d4b0155-4c13-474f-bb4f-887e2b63ba91)

### Calls

![Calls](https://github.com/user-attachments/assets/3b3fa564-d5df-4658-8882-4e21a37343d4)
