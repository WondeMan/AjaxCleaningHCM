$(document).ready(function () {
    $('#table').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": true,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "iDisplayLength": 8
    });

    @if (ViewBag.SuccessAlertMessage != null) {
        <text>
            Swal.fire({
                position: 'center',
            icon: 'success',
            title: '@ViewBag.SuccessAlertMessage',
            showConfirmButton: false,
            timer: 3000
                    })
        </text>
        ViewBag.SuccessAlertMessage = null;
    }
    @if (ViewBag.FailureAlertMessage != null) {
        <text>
            Swal.fire({
                position: 'center',
            icon: 'error',
            title: '@ViewBag.FailureAlertMessage',
            showConfirmButton: false,
            timer: 3000
                        })
        </text>
        ViewBag.FailureAlertMessage = null;
    }



});