import sys
import requests
import urllib.parse
import json
from openai import OpenAI
from openai import APIError, APIConnectionError, RateLimitError, Timeout

def get_definition(term):
    url = "https://api.urbandictionary.com/v0/define?term=" + urllib.parse.quote(term)
    headers = {'User-Agent': 'Mozilla/5.0'}
    resp = requests.get(url, headers=headers, timeout=5)
    return resp.json()

def get_ai_response(something):
    client = OpenAI(
    base_url="https://openrouter.ai/api/v1",
    api_key="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",
    )

    try:
        completion = client.chat.completions.create(
            model="deepseek/deepseek-r1-0528-qwen3-8b:free",
            messages=[
                {"role": "system", "content": "short answer, maximum 2 sentences."},
                {"role": "user", "content": something}
            ]
        )

        return completion.choices[0].message.content

    except APIError as e:
        return "API Error: ", e
    except APIConnectionError as e:
        return "Connection Error: ", e
    except RateLimitError as e:
        return "Rate Limit Error: ", e
    except Timeout as e:
        return "Request timed out: ", e

if __name__ == "__main__":
    if len(sys.argv) < 3:
        print("fail")
        sys.exit(1)

    if sys.argv[1] == "ud":
        term = sys.argv[2]
        data = get_definition(term)
        print(json.dumps(data))

    elif sys.argv[1] == "ai":
        something = sys.argv[2]
        print(get_ai_response(something))