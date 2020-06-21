"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = "Alex";
    var roomID = "ss";
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", roomID, user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

function updateScroll() {
    var element = document.getElementsByClassName("scroll");
    element.scrollTop = element.scrollHeight;
}