import sys
import requests
import urllib.parse
import json
from openai import OpenAI

def get_definition(term):
    url = "https://api.urbandictionary.com/v0/define?term=" + urllib.parse.quote(term)
    headers = {'User-Agent': 'Mozilla/5.0'}
    resp = requests.get(url, headers=headers, timeout=5)
    return resp.json()

def get_ai_response(something):
    API_KEY = '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'
    API_URL = 'https://openrouter.ai/api/v1/chat/completions'

    # Define the headers for the API request
    headers = {
        'Authorization': f'Bearer {API_KEY}',
        'Content-Type': 'application/json'
    }

    # Define the request payload (data)
    data = {
        "model": "deepseek/deepseek-chat:free",
        "messages": [
            {"role": "system", "content": "short answer, maximum 2 sentences."},
            {"role": "user", "content": something}
        ]
    }

    # Send the POST request to the DeepSeek API
    response = requests.post(API_URL, json=data, headers=headers)

    # Check if the request was successful
    if response.status_code == 200:
        result = response.json()
        message = result['choices'][0]['message']['content']
        return message
    else:
        return "Failed to fetch data from API. Status Code:", response.status_code

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