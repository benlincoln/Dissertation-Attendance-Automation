var gateway = null;
// Gets the default gateway from the Node layer, as well as verifies the connection to the API


document.onload = setTimeout(function () {
    gateway = document.getElementById("con").innerHTML;
    console.log(document.getElementById("con").innerHTML);
    baseURL = document.getElementById("baseUrl").innerHTML;
    console.log(baseURL);
    var target = document.getElementById("baseUrl");
    target.remove();
    const Http = new XMLHttpRequest();
    Http.open("GET", baseURL);
    Http.send();
    Http.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            document.getElementById("con").innerHTML = "Successfully Connected to the API";
            document.getElementById("con").style.color = "#32CD32";
        }
        else if (this.status != 200) {
            document.getElementById("con").innerHTML = "Error connecting to the API";
            document.getElementById("con").style.color = "#ff3300";
        }
    }
}, 1000);


function clicked() {
    var usr = document.getElementById('studentNo').value;
    var pswrd = document.getElementById('pass').value;
    const loginReqContents = {
        headers: {
            "username": usr,
            "password": pswrd
        },
        method: 'GET'
    }
    fetch(baseURL + '/student', loginReqContents)
        .then(response => { return response.json() })
        .then(data => {
            if (data.name == "Error") {
                // Incorrect login details throws out of the process
                document.getElementById("status").innerHTML = "Error: no match found for this username/password combination";
                document.getElementById("status").style.color = "red";
                return;
            }

            document.getElementById("status").innerHTML = "Welcome, " + data.name;
            document.getElementById("status").style.color = "black";
            document.getElementById("Attendance").innerHTML = data.attendance + "%";
            const enrolled = data.enrolledClasses;
            const getEventReqContents = {
                headers: {
                    "studentid": usr,
                    "classes": enrolled
                },
                method: 'GET'
            }
            fetch(baseURL + '/event', getEventReqContents)
                .then(response => { return response.json() })
                .then(data => {
                    console.log(data)
                    if (data.length == 0) {
                        // No next event found for the student
                        document.getElementById("nextEvent").innerHTML = "No upcoming events!";
                        document.getElementById("nextTime").innerHTML = "--"
                        return;
                    }

                    if (!data[0].current) {
                        document.getElementById("nextEvent").innerHTML = data[0].eventName;
                        document.getElementById("nextTime").innerHTML = data[0].time;
                        return;
                    }
                    var currentEventID = data[0].eventID;
                    const getLocReqContents = {
                        headers: {
                            "locationid": data[0].locationID
                        },
                        method: 'GET'
                    }
                    fetch(baseURL + '/location', getLocReqContents)
                        .then(response => { return response.json() })
                        .then(data => {
                            console.log(data.locationIP);
                            if (gateway != data.locationIP) {
                                document.getElementById("status").innerHTML = "Error: Could not mark you as present," +
                                    " please make sure you are in " + data.locationName;
                                document.getElementById("status").style.color = "red";
                                return;
                            }
                            const signInReqContents = {
                                headers: {
                                    "studentID": usr,
                                    "eventID": currentEventID
                                },
                                method: 'PATCH'
                            }
                            fetch(baseURL + '/event', signInReqContents)
                                .then(response => { return response.json() })
                                .then(data => {
                                    document.getElementById("status").innerHTML = "Successfully signed you in for this event!";
                                    document.getElementById("status").style.color = "green";
                                })
                        })
                    // Try catch to check if the user has an upcoming event
                    try {
                        document.getElementById("nextEvent").innerHTML = data[1].eventName;
                        document.getElementById("nextTime").innerHTML = data[1].time;
                    }
                    catch {
                        document.getElementById("nextEvent").innerHTML = "No upcoming events!";
                        document.getElementById("nextTime").innerHTML = "--"
                    }
                })
        }
        )
}
