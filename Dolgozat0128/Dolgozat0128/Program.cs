using Dolgozat0128.Services;

FileService fileService = new FileService();

var orders = fileService.loadData().orders;
var customers = fileService.loadData().customers;
var parts = fileService.loadData().parts;
var summary = fileService.loadData().summary;

fileService.MostSoldCustomer(orders,customers);
fileService.MostSoldProduct(orders, parts);
fileService.AllOfPayment(orders, summary);
fileService.AllOfPayment(orders, summary);