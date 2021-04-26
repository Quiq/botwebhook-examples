## 
# Basic Flask Webhook Example
#
# usage: 
# `pip install flask`
# `flask run`



from flask import Flask, request
from logging.config import dictConfig

# Configure logger
dictConfig({
    'version': 1,
    'formatters': {'default': {
        'format': '[%(asctime)s] %(levelname)s in %(module)s: %(message)s',
    }},
    'handlers': {'wsgi': {
        'class': 'logging.StreamHandler',
        'stream': 'ext://flask.logging.wsgi_errors_stream',
        'formatter': 'default'
    }},
    'root': {
        'level': 'INFO',
        'handlers': ['wsgi']
    }
})

app = Flask(__name__)

@app.route('/', methods=["GET","POST"])
def hello_world():
    ##
    # Receive a webhook json payload: More details -> (https://support.goquiq.com/api/docs#tag/Bot-Webhook)
    # {
    #   "conversation": {
    #     "id": <id>,
    #      .... 
    #   }
    #   "derivedData": {
    #     "lastCustomerMessage": {...}
    #   }
    ##
    update = request.json
    app.logger.info("Recieved webhook for conversation %s", update["conversation"]["id"])

    ##
    # Build a response object: More details -> (https://support.goquiq.com/api/docs#tag/Bot-Webhook)
    ##
    resp = {
      "actions": [{ 
        "action": "sendMessage",
        "message": {
          "default": {
            "text": "Hello World!"
          },
          "overrides": {
            "SMS": {"text": "Hello SMS User!"},
          }
        }
      }
      #,{ 
      #  "action": "setField",
      #  "field": "schema.conversation.customer.firstName",
      #  "value": "Example"
      #}
      # ... other actions
      ]
    }

    return resp
