$(function () {
    $("input:file").change(function () {
        var fileName = $(this).val();
        //alert(fileName);
        var formData = new FormData();
        formData.append('file', $('#file')[0].files[0]);
        $.ajax({
            url: "/setup/temp",
            type: "POST",
            data: formData,
            processData: false,  // tell jQuery not to process the data
            contentType: false,  // tell jQuery not to set contentType
            success: function (data) {
                $("#thumbnail").attr("src", data);
            }
        });
        //$("#fileform1").submit();
    });
});



$(function () {


    $("#DateOfBirth").datepicker();
    $("#PaymentDate").datepicker();
    //console.log($("#PaymentDate"));
    $("#JoiningDate").datepicker();
    $("#EndDate").datepicker();

    var places = ["District East Karachi", "District West Karachi", "District Central Karachi", "District Korangi", "District South Karachi"];
    $("#PlaceOfBirth").autocomplete({
        source: places
    });
    var relations = ["Brother", "Sister", "Paternal Uncle", "Maternal Uncle", "Paternal Aunt", "Maternal Aunt"];
    $("#Guardian__Relation").autocomplete({
        source: relations
    });
    var religions = ["Islam", "Christianity", "Hinduism"];
    $("#Religion").autocomplete({
        source: religions
    });
    var languages = ["Urdu", "Sindhi", "Pushto", "Punjabi", "Siraeki", "Balochi"];
    $("#MotherTongue").autocomplete({
        source: languages
    });
    var bloodGroups = ["A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"];
    $("#BloodGroup").autocomplete({
        source: bloodGroups
    });
    var nationalities = ["Pakistani",];
    $("#Nationality").autocomplete({
        source: nationalities
    });
    var admissionBasis_ = ["Merit Based", "Need Based"];
    $("#AdmissionBasis").autocomplete({
        source: admissionBasis_
    });
    var paymentModes = ["Monthly", "Quarterly", "Semi Annually", "Annually"];
    $("#Enrolment_PaymentMode").autocomplete({
        source: paymentModes
    });
    var cities = ["Karachi", "Hyderabad"];
    $("#City").autocomplete({
        source: cities
    });
    var professions = ["Doctor", "Engineer", "Teacher", "Businessman", "No Profession", "Factory Worker", "Government Servant"];
    $("#Father__Profession").autocomplete({
        source: professions
    });
    $("#Mother__Profession").autocomplete({
        source: professions
    });
    $("#Guardian__Profession").autocomplete({
        source: professions
    });
    var orgtypes = ["Public Sector", "Private Sector", "Semi-Government"];
    $("#Father__OrganizationType").autocomplete({
        source: orgtypes
    });
    $("#Mother__OrganizationType").autocomplete({
        source: orgtypes
    });
    $("#Guardian__OrganizationType").autocomplete({
        source: orgtypes
    });

    var education_ = ["Matric", "Inter", "Graduation", "Masters", "Ph.D", "BE", "ME", "BS", "MS", "MBBS"];
    $("#Father__Education").autocomplete({
        source: education_
    });
    $("#Mother__Education").autocomplete({
        source: education_
    });
    $("#Guardian__Education").autocomplete({
        source: education_
    });
});