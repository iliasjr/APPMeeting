# APPMeeting API

## AI-assisted profile matching

This API includes an AI-style matchmaking endpoint that ranks candidate profiles against a target profile.

### Endpoint

`POST /api/matching/recommendations`

### Request body

```json
{
  "targetProfile": {
    "username": "alice",
    "bio": "Product manager focused on fintech and mentoring",
    "interests": ["fintech", "networking", "mentoring"],
    "skills": ["product", "agile", "strategy"],
    "preferredMeetingType": "virtual"
  },
  "candidateProfiles": [
    {
      "username": "bob",
      "bio": "Engineer interested in fintech products and mentoring startups",
      "interests": ["fintech", "startups"],
      "skills": ["agile", "csharp"],
      "preferredMeetingType": "virtual"
    }
  ],
  "top": 3
}
```

### Response body

```json
[
  {
    "username": "bob",
    "score": 53,
    "explanation": "Shared interests (1): fintech. Shared skills (1): agile. Meeting preference match: yes. Bio similarity points: 6."
  }
]
```

### Scoring logic

- Shared interests: up to 40 points.
- Shared skills: up to 35 points.
- Same meeting type: 15 points.
- Bio keyword overlap: up to 10 points.

### Notes

- Username is required for target and candidate profiles.
- Duplicate candidates (same username) are deduplicated.
- Total score is capped at 100.
