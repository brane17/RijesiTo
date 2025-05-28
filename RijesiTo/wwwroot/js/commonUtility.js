var TaskStatus = {
    NotStarted: 0, // zadani status nakon stvaranja zadatka
    InProgress: 1, // postavlja sustav nakon što radnik prihvati ponudu
    Finished: 2, // postavio radnik
    Completed: 3, // postavio klijent nakon završetka
    Closed: 4 // sustav postavlja nakon isplate/računa
}

var OfferStatus = {
    Pending: 0,
    Accepted: 1,
    Rejected: 2
}


function getTaskStatusText(status) {
    if (status == TaskStatus.NotStarted) {
        status = 'Not Started';
    } else if (status == TaskStatus.InProgress) {
        status = 'In Progress';
    }
    else if (status == TaskStatus.Finished) {
        status = 'Finished';
    } 
    else if (status == TaskStatus.Completed) {
        status = 'Completed';
    } else {
        status = 'Unknown';
    }
    return status;
}



function getOfferStatusText(status) {
    if (status == OfferStatus.Pending) {
        status = 'Pending';
    } else if (status == OfferStatus.Accepted) {
        status = 'Accepted';
    } else if (status == OfferStatus.Rejected) {
        status = 'Rejected';
    } else {
        status = 'Unknown';
    }
    return status;
}