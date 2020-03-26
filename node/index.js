const express = require("express");
const morgan = require("morgan");
const bodyParser = require("body-parser");
const app = express();

(async () => {
  var jsonParser = bodyParser.json();

  app.use(morgan("combined"));
  app.all("/", jsonParser, async (req, res) => {
    /**
     * Receive a webhook json payload: More details -> (https://support.goquiq.com/api/docs#tag/Bot-Webhook)
     * {
     *   "conversation": {
     *     "id": <id>,
     *      ....
     *   }
     *   "derivedData": {
     *     "lastCustomerMessage": {...}
     *   }
     */
    const event = req.body;
    console.log(`Recieved webhook for conversation ${event.conversation.id}`);

    /**
     * Build a response object: More details -> (https://support.goquiq.com/api/docs#tag/Bot-Webhook)
     */
    return res.json({
      actions: [
        {
          action: "sendMessage",
          message: {
            default: {
              text: "Hello World!"
            },
            overrides: {
              SMS: {text: "Hello SMS User!"}
            }
          }
        }
        // ... other actions
      ]
    });
  });

  app.listen(3002, function() {
    console.log("Listening on port 3002");
  });
})();
