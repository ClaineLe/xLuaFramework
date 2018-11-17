
import xlrd
import csv
import codecs
import sys
import os
import shutil

reload(sys)
sys.setdefaultencoding( "utf-8" )

CUR_DIR_FULL_PATH = os.path.dirname(os.path.realpath(__file__))

PROJECT_CSV_FULL_PATH = CUR_DIR_FULL_PATH + "/../Assets/AppAssets/Xls"

EXCEL_DIR_PATH = CUR_DIR_FULL_PATH + "/excel"
EXCEL_SUFFIX_NAME = ".xlsx"

CSV_DIR_PATH = CUR_DIR_FULL_PATH + "/csv"
CSV_SUFFIX_NAME = ".csv"

def sheet_to_csv(xlsx_name,sheet):
	try:
		if sheet.nrows < 1 and sheet.ncols < 1:
			return
		csv_file_name = sheet.cell_value(1, 1)
		if csv_file_name.strip():
			csv_full_path = CSV_DIR_PATH + "/" + csv_file_name + CSV_SUFFIX_NAME
			with codecs.open(csv_full_path, 'w', encoding='utf-8') as f:
				write = csv.writer(f)
				for row_num in range(sheet.nrows):
					row_value = sheet.row_values(row_num)
					write.writerow(row_value)
				print("Export Success: => " + sheet.name + "(" + csv_file_name + ")")
	except BaseException,err:
		print("---------------------------------------------------")
		print("[Export Excel To Csv Error]")
		print("[xlsx] " + xlsx_name)
		print("[sheet] " + sheet.name)
		print(err)
		print("---------------------------------------------------")

		os.system("pause")
		os.exit()

def excel_to_sheet(xlsx_name):
	xlsx_full_path = EXCEL_DIR_PATH + "/" + xlsx_name
	workbook = xlrd.open_workbook(xlsx_full_path)
	for sheet in workbook.sheets():
		if sheet.visibility == 0:
			if not sheet.name.startswith("#"):
				sheet_to_csv(xlsx_name,sheet)

def collect_all_excel():
	for root, dirs, files in os.walk( EXCEL_DIR_PATH):  
		for file in files:  
			if os.path.splitext(file)[1] == EXCEL_SUFFIX_NAME:  
				if not file.startswith("~$"):
					excel_to_sheet(file)

def delete_all_csv(dest_path):
    ls = os.listdir(dest_path)
    for i in ls:
        c_path = os.path.join(dest_path, i)
        if os.path.isdir(c_path):
            del_file(c_path)
        else:
            os.remove(c_path)
    print("clear all csv success!")

def copy_csv_to_project():
	for root, dirs, files in os.walk( CSV_DIR_PATH):  
		for file in files:  
			if os.path.splitext(file)[1] == CSV_SUFFIX_NAME:  
				scr_full_path = CSV_DIR_PATH + "/" + file
				dst_full_path = PROJECT_CSV_FULL_PATH + "/" + file
				shutil.copy(scr_full_path,dst_full_path)

if __name__ == '__main__':
	print(PROJECT_CSV_FULL_PATH)
	delete_all_csv(CSV_DIR_PATH)
	collect_all_excel()
	delete_all_csv(PROJECT_CSV_FULL_PATH)
	copy_csv_to_project()
	print("export excel to csv finish !!!!!!!!!!")
	os.system("pause")