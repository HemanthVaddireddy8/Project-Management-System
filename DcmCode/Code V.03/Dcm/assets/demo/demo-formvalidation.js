$(function() {
	//Parsley Form Validation
    //While the JS is not usually required in Parsley, we will be modifying
    //the default classes so it plays well with Bootstrap
    $('#validate-form').parsley({
        successClass: 'has-success',
        errorClass: 'has-error',
        errors: {
            classHandler: function(el) {
                return $(el).closest('.form-validate');
            },
            errorsWrapper: '<ul class=\"help-block list-unstyled\"></ul>',
            errorElem: '<li></li>'
        }
    });
});