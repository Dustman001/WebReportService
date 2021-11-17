var connection = new signalR.HubConnectionBuilder()
    .withUrl("/DataHub")
    .build();

connection.start().then(() => {
    textarea.innerText += "Conextion existosa/n";
}).catch(err => {
    textarea.innerText += err;
});

connection.on("ReceiveData", function (data) {

    var textarea = document.getElementById("texto");   

    const djson = JSON.parse(data);

    var dreport = djson.reports;

    for (var val in dreport) {
        document.getElementById('tab1').innerHTML += '<tr style="text-align: center;"><td style="text-align: center; border: 1px solid black; width: 33%;">' + dreport[val].Name + '</td><td style="border: 1px solid black; width: 33%;" id="' + dreport[val].Ids + '">' + dreport[val].Status + '</td><td style="border: 1px solid black; width: 34%;"><button name="' + dreport[val].Ids + '" onclick="checkstatus(this);">' + dreport[val].Command + '</button></td></tr>';
    }

    var dmessage = djson.message;
    var dmes = "";

    for (var val in dmessage) {
        dmes += dmessage[val];
    }    

    textarea.innerText = dmes;

    /*
    document.getElementById('tab1').innerHTML = "";
    for (var val in dreport) {        
        document.getElementById('tab1').innerHTML += '<tr style="text-align: center;">< td style = "text-align: center; border: 1px solid black; width: 33%;" > ' + dreport[val].name + '</td >< td style = "border: 1px solid black; width: 33%;" id = "' + dreport[val].ids + '" > ' + dreport[val].status + '</td><td style="border: 1px solid black; width: 34%;">< button name = "' + dreport[val].ids + '" onclick = "checkstatus(this);" >' + dreport[val].command + '</button ></td ></tr >';
    }
    */
});