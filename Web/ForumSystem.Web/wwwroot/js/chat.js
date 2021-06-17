const sendBtn = document
    .getElementById("sendButton");
const messageInput = document
    .getElementById("messageInput");

const greetingMsg = document
    .getElementById("dropdownMenuLink").textContent
    .trim();

let connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/category-chat", {
            skipNegotiation: true,
            transport: signalR.HttpTransportType.WebSockets
        })
        .build();

connection.on("NewMessage",
    function (messageObj) {
        let chatInfo;

        if (greetingMsg.includes(messageObj.user))
            chatInfo = createCurrUserChatInfo(messageObj);
        else chatInfo = createOtherUserChatInfo(messageObj);

        document.getElementById("messagesList").innerHTML += chatInfo;
    });

connection
    .start()
    .catch(function (err) {
        return console.error(err.toString());
    });

sendBtn
    .addEventListener("click", sendMessage);

messageInput
    .addEventListener("keyup", stoppedTyping);

messageInput
    .addEventListener("keyup", stoppedTyping);

function stoppedTyping() {
    var currMessage = this.value.trim();

    if (currMessage.length > 0) {
        sendBtn.disabled = false;
    } else {
        sendBtn.disabled = true;
    }

    if (event.keyCode === 13) {
        sendBtn.click();
    }
}

function sendMessage(target) {
    target.preventDefault();

    let locationParts = location.href.split("/");

    let categoryName = locationParts[locationParts.length - 1];

    let message = messageInput.value;

    if (message != "") {
        connection.invoke("Send", message, categoryName);
        messageInput.value = "";
        sendBtn.disabled = true;
    }
}

function createOtherUserChatInfo(messageObj) {
    let element = `<div class="media w-50 mb-3">
                                            <img src="/profileImages/12f9d30c-5b51-4cbf-8485-6cf725db73f0.png" alt="user" width="50" class="rounded-circle">
                                            <div class="media-body ml-3">
                                                <div class="bg-light rounded py-2 px-3 mb-2">
                                                    <p class="text-small mb-0 text-muted">${messageObj.user}</p>
                                                    <p class="text-small mb-0 text-muted">${escapeHtml(messageObj.content)}</p>
                                                </div>
                                                <p class="small text-muted">
                                                     <time datetime="${messageObj.createdOn.toISOString()}"></time>
                                                </p>
                                            </div>
                                        </div>`

    return element;
}

function createCurrUserChatInfo(messageObj) {
    let element = `<div class="media w-50 ml-auto mb-3">
                                    <div class="media-body">
                                        <div class="bg-primary rounded py-2 px-3 mb-2">
                                            <p class="text-small mb-0 text-white">${escapeHtml(messageObj.content)}</p>
                                        </div>
                                       <p class="small text-muted">
                                            <time datetime="${messageObj.createdOn.toISOString()}"></time>
                                       </p>
                                    </div>
                                </div>`

    return element;
}

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}