# 🎙️ Voice AI Product Assistant

An intelligent real-time Voice AI Assistant built with **ASP.NET Core**, **Google Gemini**, and **browser-based speech capabilities**. The assistant supports natural conversations, remembers previous context, intelligently decides when to retrieve product information or call tools, and delivers low-latency responses for a seamless voice experience.

---

## 🚀 Features

| Module | Status |
|--------|--------|
| Conversation Memory | ✅ |
| Product Retrieval | ✅ |
| Tool Calling | ✅ |
| Intelligent Router | ✅ |
| Follow-up Memory | ✅ |
| Streaming Responses | ✅ |
| Prompt Engineering | ✅ |
| Error Handling | ✅ |
| Browser Voice Integration | ✅ |
| Natural Conversation | ✅ |
| Low Latency Response | ✅ |

---

## ✨ Key Capabilities

- 🎤 Real-time browser voice interaction
- 🧠 Context-aware conversation memory
- 🔍 Intelligent product retrieval
- 🤖 Google Gemini powered reasoning
- 🔧 Dynamic tool calling architecture
- 🧭 AI Router for deciding the best response strategy
- 💬 Multi-turn conversation support
- ⚡ Fast streaming responses
- 🛡 Robust error handling and fallback responses
- 📦 Modular service architecture
- 🌐 REST API backend
- 📱 Responsive web interface

---

# Architecture

```
Browser
   │
   ▼
Speech Recognition
   │
   ▼
ASP.NET Core API
   │
   ▼
Conversation Service
   │
   ├───────────────┐
   ▼               ▼
AI Router      Product Search
   │               │
   ▼               ▼
Gemini AI      Product Tools
   │               │
   └──────┬────────┘
          ▼
    AI Response
          │
          ▼
Text-to-Speech
          │
          ▼
       Browser
```

---

# Technology Stack

## Backend

- ASP.NET Core
- C#
- REST APIs
- Dependency Injection

## AI

- Google Gemini API
- Prompt Engineering
- Function Calling
- Conversational Memory

## Frontend

- HTML5
- CSS3
- JavaScript
- Browser Speech Recognition API
- Speech Synthesis API

---

# Project Structure

```
VoiceAIAgent
│
├── Controllers/
├── Services/
│     ├── ConversationService
│     ├── GeminiService
│     ├── ProductService
│     ├── ProductToolService
│     ├── ToolRouterService
│     └── AIPipelineService
│
├── Interfaces/
├── Models/
├── Tools/
├── Data/
├── wwwroot/
├── Program.cs
└── appsettings.json
```

---

# AI Pipeline

User Voice

↓

Speech Recognition

↓

Conversation Memory

↓

Intelligent Router

↓

Gemini AI

↓

Tool Calling (If Required)

↓

Product Retrieval (If Required)

↓

AI Response

↓

Speech Synthesis

---

# Installation

Clone the repository

```bash
git clone https://github.com/YOUR_USERNAME/VoiceAIAgent.git
```

Navigate to the project

```bash
cd VoiceAIAgent
```

Restore packages

```bash
dotnet restore
```

Run the application

```bash
dotnet run
```

---

# Configuration

Update your `appsettings.json`

```json
{
  "Gemini": {
    "ApiKey": "YOUR_GEMINI_API_KEY"
  }
}
```

For production deployment, store API keys as **environment variables** instead of committing them to source control.

---

# API Workflow

1. User speaks into browser
2. Speech converted to text
3. Conversation context loaded
4. AI Router selects processing strategy
5. Gemini generates reasoning
6. Product tools invoked when required
7. Response streamed back
8. Browser converts response to speech

---

# Performance Highlights

- Low response latency
- Context-aware conversations
- Modular architecture
- Scalable service layer
- Efficient tool routing
- Clean separation of concerns

---

# Future Enhancements

- Authentication & User Profiles
- Vector Database Integration
- Retrieval-Augmented Generation (RAG)
- Multi-language Voice Support
- Voice Personalization
- Analytics Dashboard
- Plugin Architecture
- Multi-Agent Collaboration
- Cloud Storage Integration
- Real-time WebSocket Streaming

---

# Deployment

The application is cloud-ready and can be deployed on platforms such as:

- Render
- Azure App Service
- AWS Elastic Beanstalk
- Docker
- Kubernetes

---

# License

This project is intended for educational, research, and demonstration purposes.

---

# Author

**Naman Saxena**

AI & Full Stack Developer

Specializing in:

- Artificial Intelligence
- ASP.NET Core
- LLM Applications
- AI Automation
- REST APIs
- Cloud Deployment

---
