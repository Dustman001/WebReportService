﻿function checkstatus(e) {
    var tdl = document.getElementById(e.name);

    if (e.innerHTML == 'Habilitar') {

        fetch(window.location.href + 'api/v1/status/' + e.name, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(e.innerHTML)
        })
            .then(response => {

                if (response.ok)
                    return response.json();
                else
                    alert(response.status);
            })
            .then(json => {
                if (json.statuscode == 200) {
                    //alert('El ' + json.report + ' ha sido ' + json.response);
                    tdl.innerHTML = 'Habilitado';
                    e.innerHTML = 'Deshabilitar';
                } else
                    alert('Error Reset ' + json.statuscode);
            })
            .catch(error => alert(error));
    }
    else {
        fetch(window.location.href + 'api/v1/status/' + e.name, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(e.innerHTML)
        })
            .then(response => {

                if (response.ok)
                    return response.json();
                else
                    alert(response.status);
            })
            .then(json => {
                if (json.statuscode == 200) {
                    //alert('El ' + json.report + ' ha sido ' + json.response);
                    tdl.innerHTML = 'Deshabilitado';
                    e.innerHTML = 'Habilitar';
                } else
                    alert('Error Reset 1' + json.statuscode);
            })
            .catch(error => alert(error));
    }
}

fetch(window.location.href + 'api/v1/status', {
    method: 'GET',
    headers: {
        'Content-Type': 'application/json'
    }
})
    .then(response => {

        if (response.ok)
            return response.json();
        else
            alert('Error 2: ' + response.status);
    })
    .then(json => {
        if (json.statuscode == 200) {
            document.getElementById('tab1').innerHTML = '<tr style="text-align: center;"><td style="border: 1px solid black; " colspan="3"><h1 class="display - 4">Estado Servicios de Reporte</h1></td></tr>';
            for (var val in json.reports) {
                document.getElementById('tab1').innerHTML += '<tr style="text-align: center;"><td style="text-align: center; border: 1px solid black; width: 33%;">' + json.reports[val].name + '</td><td style="border: 1px solid black; width: 33%;" id="' + json.reports[val].ids + '">' + json.reports[val].status + '</td><td style="border: 1px solid black; width: 34%;"><button name="' + json.reports[val].ids + '" onclick="checkstatus(this);">' + json.reports[val].command + '</button></td></tr>';
            }
            //document.getElementById('test1').innerHTML = 'Name: ' + val0.Name + ', Ids: ' + val0.Ids + ', Status: ' + val0.Status;

        } else {
            alert('Error Reset 2: ' + json);
            alert('Error Reset 3: ' + json.statuscode);
            alert('Error Reset 4: ' + json.reports);
        }
    })
    .catch(error => alert(error));