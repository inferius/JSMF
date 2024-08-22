var a = 0;
const cstr = `ahoj ${a} test`
var ac = 56;
var c = {
    ahoj: {
        "test": false,
        ppp_sf: 1
    },
    "aaa": "sfs"
}
let x = 5;

var z = 5, zz = 6; // TODO: Opravit, promena zz se vytvori jako NodeIdentifier misto NodeDefVar pak to padne na prirazeni neexistujici promenne, bude treba upravit parser
//var z = 5;

function test(ss) {
    let test = 55;
    while (true) {
        /*
        TODO: Fix syntax error z++ == 5 a ++z == 5, pada na syntax error
        if (z++ == 5) {
            continue;
        }
        z = 5;
        if (++z == 5) {
            break;
        }
        */
        break;
    }
    z++;
    ++z;
    // return tes; // Vraci korektne undefined variable
    return test + z;
}

async function testAync() {
    'use strict';
    var a = () => test;
    let b = (a) => test;
    var c = (c, d) => test;
    var d = (a) => { return t; };
    var x = a => { return; }
    var z = a => b;
    await test();

    if (a == 5) {
        return c["ahoj"].ppp_sf[x()];
    }

    for (var index = 0; index < 10; index++)
        console.log(index);
    
    for (var i = 0, j = 0; i < 10; i++, j++)
        console.log(j);


    return c.ahoj.ppp_sf;
}
test();
console.log(cstr); // vystup "ahoj 0 test"
a = 5;
console.log(cstr);  // vystup "ahoj 0 test"

let strtest = 5;
const getCstr = () => `ahoj ${strtest} test`;
console.log(getCstr());  // Výstup: "ahoj 5 test"

strtest = 10;
console.log(getCstr());  // Výstup: "ahoj 10 test"

//function* generator() {
//    yield 42;
//}

for (let i = 0, j = 0; i < 10; i++, j+=2)
    console.log(`iteration: j = ${j}, i = ${i}`);

(function(a, b) {
    for (var i = 0, h = 6; i < 10; i++) console.log(i);
    const x = [1, 2, 4, 5, 6];
    for (const v of x) {
        console.log(v);
    }
})(1, 2);

(async function (a, b) {
    for (var i = 0, h = 6; i < 10; i++) console.log(i);
    const x = [1, 2, 4, 5, 6];
    for (const v of x) {
        console.log(v);
    }
})(1, 2);

((a, b) => {
    for (var i = 0, h = 6; i < 10; i++) console.log(i);
    const x = [1, 2, 4, 5, 6];
    for (const v of x) {
        console.log(v);
    }
})(1, 2);

(async (a, b) => {
    for (var i = 0, h = 6; i < 10; i++) console.log(i);
    const x = [1, 2, 4, 5, 6];
    for (const v of x) {
        console.log(v);
    }
})(1, 2);

(async (a, b) => {
    for (var i = 0, h = 6; i < 10; i++) console.log(i);
    const x = [1, 2, 4, 5, 6];
    for (const v of x) {
        console.log(v);
    }
})

function two_same_let() {
    let a = 5;
    let a = 6;
    // Syntax error
}
/*
class Test extends Test1 {
    get testGet() { }
    set testSet(v) { }

    constructor() {
        super();
        this.test = 5;
    }

    testMethod(a, b = 5) {
        super.testMethod();
        this.test = b;
    }

    static async testStaticAsync() {
        
    }