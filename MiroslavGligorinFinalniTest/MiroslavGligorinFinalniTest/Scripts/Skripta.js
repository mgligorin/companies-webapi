$(document).ready(function () {

    // podaci od interesa
    var host = window.location.host;
    var token = null;
    var headers = {};
    var zaposleniEndpoint = "/api/zaposleni/";
    var editingId;
    var formAction = "Create";

    // okidanje ucitavanja proizvoda
    loadZaposleni();

    // posto inicijalno nismo prijavljeni, sakrivamo odjavu
    $("#odjava").css("display", "none");

    // pripremanje dogadjaja za izmenu i brisanje
    $("body").on("click", "#btnDelete", deleteZaposleni);
    $("body").on("click", "#btnEdit", editZaposleni);
    $("body").on("click", "#btnOdustajanje", formaOdustajanje);
    $("body").on("click", "#btnReset", loadZaposleni);

    $("body").on("click", "#btnRegistracijaIPrijava", registracijaIPrijava); 
    $("body").on("click", "#btnPocetak", pocetak);
    $("body").on("click", "#btnRegistracija", registracijaForma);
    $("body").on("click", "#btnPrijava", prijavaForma);

    function registracijaIPrijava() {
        $("#registracijaIPrijava").css("display", "none");
        $("#pocetak").css("display", "block");
        $("#registracija").css("display", "block");
    }

    function pocetak() {
        $("#pocetak").css("display", "none");
        $("#registracijaIPrijava").css("display", "block");
        $("#registracija").css("display", "none");
        $("#prijava").css("display", "none");
    }

    function registracijaForma() {
        $("#prijava").css("display", "none");
        $("#registracija").css("display", "block");
    }

    function prijavaForma() {
        $("#prijava").css("display", "block");
        $("#registracija").css("display", "none");
    }

    // ucitavanje zaposlenih
    function loadZaposleni() {
        var requestUrl = 'http://' + host + zaposleniEndpoint;
        $.getJSON(requestUrl, setZaposleni);
    }

    // metoda za postavljanje zaposlenog u tabelu
    function setZaposleni(data, status) {
        
        var $container = $("#zaposleni");
        $container.empty();

        if (status === "success") {
            // ispis naslova
            var div = $("<div></div>");
            var h1 = $("<h1>Zaposleni</h1>");
            div.append(h1);
            // ispis tabele
            var table = $("<table class='table table-bordered'></table>");
            if (token) {
                var header = $("<thead><tr><td>Ime i prezime</td><td>Godina rodjenja</td><td>Godina zaposlenja</td><td>Kompanija</td><td>Plata</td><td>Brisanje</td><td>Izmena</td></tr></thead>");
            } else {
                var header = $("<thead><tr><td>Ime i prezime</td><td>Godina rodjenja</td><td>Godina zaposlenja</td><td>Kompanija</td></tr></thead>");
            }
            
            table.append(header);
            tbody = $("<tbody></tbody>");
            for (i = 0; i < data.length; i++) {
                // prikazujemo novi red u tabeli
                var row = "<tr>";
                // prikaz podataka
                var displayData = "<td>" + data[i].ImeIPrezime + "</td><td>" + data[i].GodinaRodjenja + "</td><td>" + data[i].GodinaZaposlenja + "</td><td>" + data[i].Kompanija.Naziv + "</td>";
                // prikaz dugmadi za izmenu i brisanje
                var stringId = data[i].Id.toString();
                var displayDelete = "<td><button id=btnDelete name=" + stringId + ">Delete</button></td>";
                var displayEdit = "<td><button id=btnEdit name=" + stringId + ">Edit</button></td>";
                // prikaz samo ako je korisnik prijavljen
                if (token) {
                    row += displayData + "<td>" + data[i].Plata + "</td>" + displayDelete + displayEdit + "</tr>";
                } else {
                    row += displayData + "</tr>";
                }
                // dodati red
                tbody.append(row);
            }
            table.append(tbody);

            div.append(table);
           
            // ispis novog sadrzaja
            $container.append(div);
        }
        else {
            var div = $("<div></div>");
            var h1 = $("<h1>Greška prilikom preuzimanja Zaposlenih!</h1>");
            div.append(h1);
            $container.append(div);
        }
    }

    // registracija korisnika
    $("#registracija").submit(function (e) {
        e.preventDefault();

        var email = $("#regEmail").val();
        var loz1 = $("#regLoz").val();
        var loz2 = $("#regLoz2").val();

        // objekat koji se salje
        var sendData = {
            "Email": email,
            "Password": loz1,
            "ConfirmPassword": loz2
        };

        $.ajax({
            type: "POST",
            url: 'http://' + host + "/api/Account/Register",
            data: sendData

        }).done(function (data) {
            $("#info").append("Uspešna registracija. Možete se prijaviti na sistem.");
            $("#regEmail").val('');
            $("#regLoz").val('');
            $("#regLoz2").val('');

        }).fail(function (data) {
            alert(data);
        });
    });

    // prijava korisnika
    $("#prijava").submit(function (e) {
        e.preventDefault();

        var email = $("#priEmail").val();
        var loz = $("#priLoz").val();

        // objekat koji se salje
        var sendData = {
            "grant_type": "password",
            "username": email,
            "password": loz
        };

        $.ajax({
            "type": "POST",
            "url": 'http://' + host + "/Token",
            "data": sendData

        }).done(function (data) {
            console.log(data);
            $("#info").empty().append("Prijavljen korisnik: " + data.userName);
            token = data.access_token;
            $("#prijava").css("display", "none");
            $("#registracija").css("display", "none");
            $("#odjava").css("display", "block");
            $("#formsearch").css("display", "block");
            $("#priEmail").val('');
            $("#priLoz").val('');
            refreshTable();

        }).fail(function (data) {
            alert(data);
        });
    });

    // odjava korisnika sa sistema
    $("#odjavise").click(function () {
        token = null;
        headers = {};

        $("#prijava").css("display", "none");
        $("#registracija").css("display", "none");
        $("#registracijaIPrijava").css("display", "block");
        $("#pocetak").css("display", "none");
        $("#odjava").css("display", "none");
        $("#info").empty();
        $("#sadrzaj").empty();
        $("#formZaposleniDiv").css("display", "none");
        $("#formsearch").css("display", "none");
        refreshTable();
    });

    // forma za rad sa zaposlenima
    $("#zaposleniForm").submit(function (e) {
        // sprecavanje default akcije forme
        e.preventDefault();

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var imeIPrezime = $("#imeIPrezime").val();
        var rodjenje = $("#rodjenje").val();
        var zaposlenje = $("#zaposlenje").val();
        var plata = $("#plata").val();
        var kompanija = $("#kompanija").val();
        var httpAction;
        var sendData;
        var url;

        // u zavisnosti od akcije pripremam objekat
        if (formAction === "Create") {
            httpAction = "POST";
            url = 'http://' + host + zaposleniEndpoint;
            sendData = {
                "ImeIPrezime": imeIPrezime,
                "GodinaRodjenja": rodjenje,
                "GodinaZaposlenja": zaposlenje,
                "Plata": plata,
                "KompanijaId": kompanija
            };
        }
        else {
            httpAction = "PUT";
            url = 'http://' + host + zaposleniEndpoint + editingId.toString();
            sendData = {
                "Id": editingId,
                "ImeIPrezime": imeIPrezime,
                "GodinaRodjenja": rodjenje,
                "GodinaZaposlenja": zaposlenje,
                "Plata": plata,
                "KompanijaId": kompanija
            };
        }

        // izvrsavanje AJAX poziva
        $.ajax({
            url: url,
            type: httpAction,
            headers: headers,
            data: sendData
        })
            .done(function (data, status) {
                formAction = "Create";
                refreshTable();
                $("#formZaposleniDiv").css("display", "none");
            })
            .fail(function (data, status) {
                alert("Greška prilikom izmene!");
            });
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        var godinaOd = $("#godinaOd").val();
        var godinaDo = $("#godinaDo").val();

        var httpAction;
        var sendData;
        var url;

        httpAction = "POST";
        url = 'http://' + host + "/api/Zaposlenje/";
        sendData = {
            "Pocetak": godinaOd,
            "Kraj": godinaDo
        };

        $.ajax({
            url: url,
            type: httpAction,
            headers: headers,
            data: sendData
        })
            .done(function (data, status) {
                $("#godinaOd").val('');
                $("#godinaDo").val('');
                setZaposleni(data, status);
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            });
    });

    // brisanje zaposlenih
    function deleteZaposleni() {
        // izvlacimo {id}
        var deleteID = this.name;

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
        }

        // saljemo zahtev 
        $.ajax({
            url: 'http://' + host + zaposleniEndpoint + deleteID.toString(),
            type: "DELETE",
            headers: headers
        })
            .done(function (data, status) {
                refreshTable();
            })
            .fail(function (data, status) {
                alert("Desila se greska!");
            });
    }

    // izmena zaposlenih
    function editZaposleni() {
        // izvlacimo id
        var editId = this.name;

        // korisnik mora biti ulogovan
        if (token) {
            headers.Authorization = 'Bearer ' + token;
            $("#formZaposleniDiv").css("display", "block");
        }

        var selectList = $("#kompanija");
        selectList.empty();
        $.ajax({
            url: 'http://' + host + '/api/kompanije',
            type: "GET",
            headers: headers
        }).done(function (kompanije, status) {
            for (i = 0; i < kompanije.length; i++) {
                if (editId === kompanije[i].Id) {
                    var displayData = '<option selected value=' + kompanije[i].Id + '>' + kompanije[i].Naziv + '</option>';
                }
                else {
                    var displayData = '<option value=' + kompanije[i].Id + '>' + kompanije[i].Naziv + '</option>';
                }
                selectList.append(displayData);
            }
            })
            .fail(function (kompanije, status) {
                formAction = "Create";
                alert("Desila se greska!");
            });

        // saljemo zahtev da dobavimo zaposlenog
        $.ajax({
            url: 'http://'+ host + zaposleniEndpoint + editId.toString(),
            type: "GET",
            headers: headers
        })
            .done(function (data, status) {
                $("#imeIPrezime").val(data.ImeIPrezime);
                $("#rodjenje").val(data.GodinaRodjenja);
                $("#zaposlenje").val(data.GodinaZaposlenja);
                $("#plata").val(data.Plata);
                $("#kompanija").val(data.KompanijaId);
                editingId = data.Id;
                formAction = "Update";
            })
            .fail(function (data, status) {
                formAction = "Create";
                alert("Greška prilikom izmene!");
            });
    }

    // osvezi prikaz tabele
    function refreshTable() {
        // cistim formu
        $("#imeIPrezime").val('');
        $("#rodjenje").val('');
        $("#zaposlenje").val('');
        $("#plata").val('');
        $("#kompanija").val('');
        // osvezavam
        loadZaposleni();
    }

    function formaOdustajanje() {
        $("#imeIPrezime").val('');
        $("#rodjenje").val('');
        $("#zaposlenje").val('');
        $("#plata").val('');
        $("#kompanija").val('');
        loadZaposleni();
        $("#formZaposleniDiv").css("display", "none");
    }
});