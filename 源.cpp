//#include "backend/Backend.h"
#include "backend/Backend.h"
#include <iostream>
using namespace std;

int main() {
	Item item("2896839052"), item2("2900400992");
	item.parse();
	item2.parse();

	ItemEntry entry;
	entry.add(item);
	entry.writeJson("ItemEntry.json");

	entry.clear();
	entry.readJson("ItemEntry.json");
	entry.add(item2);
	entry.writeJson("ItemEntry.json");
	return 0;
}