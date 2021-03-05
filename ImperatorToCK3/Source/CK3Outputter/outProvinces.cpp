#include "outProvinces.h"
#include "CK3/Province/CK3Province.h"
#include "CommonFunctions.h"
#include <filesystem>
#include <fstream>



void CK3::outputHistoryProvinces(const std::string& outputModName, const std::map<unsigned long long, std::shared_ptr<Province>>& provinces) {
	std::ofstream output("output/" + outputModName + "/history/provinces/province_history.txt"); // dumping all into one file
	if (!output.is_open())
		throw std::runtime_error("Could not create province history file: output/" + outputModName + "/history/provinces/province_history.txt");

	output << "# number of provinces: " << provinces.size() << "\n";
	for (const auto& [unused, provincePtr] : provinces) {
		output << *provincePtr;
	}
	output.close();

	//create province mapping dummy
	std::ofstream dummy("output/" + outputModName + "/history/province_mapping/dummy.txt");
	if (!dummy.is_open())
		throw std::runtime_error(
			"Could not create province mapping file: output/" + outputModName + "/history/province_mapping/dummy.txt");
	dummy << commonItems::utf8BOM;
	dummy.close();
}
