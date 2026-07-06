# 🎤 Voice AI Assistant

A Voice AI Assistant built with ASP.NET Core that demonstrates AI-powered product search, conversational memory, speech recognition, speech synthesis and a RAG-style knowledge retrieval pipeline.

## Features

- 🎤 Browser Speech Recognition
- 🔊 Text-to-Speech Responses
- 💬 Conversational Chat UI
- 🧠 Conversation Memory
- 📚 RAG-style Knowledge Retrieval
- ⚡ Fast Product Search
- 📝 Typing Animation
- 🌐 REST API
- 📦 Product Knowledge Base

## Tech Stack

- ASP.NET Core 8
- C#
- Swagger
- HTML
- CSS
- JavaScript
- Web Speech API
- REST APIs

## Architecture

Voice Input
↓
Speech Recognition
↓
ASP.NET Core API
↓
Knowledge Retrieval (RAG-style)
↓
Response Generation
↓
Typing Animation
↓
Speech Synthesis

## API Endpoint

POST

```
/api/chat/ask
```

Example Request

```json
{
  "message": "Tell me about Galaxy S25"
}
```

## Future Improvements

- OpenAI GPT Integration
- OpenAI Realtime API
- Twilio Voice Integration
- Vector Database
- Semantic Search
- Streaming Responses
- Live Voice Interruptions

## Author

Naman Saxena
