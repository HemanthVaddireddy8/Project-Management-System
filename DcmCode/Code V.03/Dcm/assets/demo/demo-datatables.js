// -------------------------------
// Initialize Data Tables
// -------------------------------

$(document).ready(function() {
    $('.datatables').dataTable({
        "sDom": "<'row'<'col-xs-6'l><'col-xs-6'f>r>t<'row'<'col-xs-6'i><'col-xs-6'p>>",
        "sPaginationType": "bootstrap",
        "oLanguage": {
            "sLengthMenu": "_MENU_ record by page",
            "sZeroRecords": "No records.",
            "sInfo": "( _START_ - _END_ ) / _TOTAL_ record",
            "sInfoFiltered": "(filtered from _MAX_ total records)"
        }
    });
    $('.dataTables_filter input').addClass('form-control').attr('placeholder','Search...');
    $('.dataTables_length select').addClass('form-control');
});