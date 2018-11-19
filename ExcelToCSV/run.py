
import xlrd
import sys
import os
import shutil

reload(sys)
sys.setdefaultencoding("utf-8")

CUR_DIR_FULL_PATH = os.path.dirname(os.path.realpath(__file__))

EXCEL_DIR_PATH = CUR_DIR_FULL_PATH + "/excel"
CSV_DIR_PATH = CUR_DIR_FULL_PATH + "/csv"
PROJECT_CSV_FULL_PATH = CUR_DIR_FULL_PATH + "/../Assets/AppAssets/#Xls"

USE_COL_NUM = 2

USE_CLIENT_NUMFLAG = 1
USE_SERVER_NUMFLAG = 2
USE_ALL_NUMFLAG = 3

def save_csv(csv_file_name, csv_data):
	csv_full_path = CSV_DIR_PATH + "/" + csv_file_name + ".csv"
	file = open(csv_full_path, 'w')
	file.write(csv_data)

def sheet_to_csv(xlsx_name,sheet):
	try:
		if sheet.nrows < 1 and sheet.ncols < 1:
			return
		csv_file_name = sheet.cell_value(0, 0)
		if csv_file_name.strip():
			use_cols = collect_use_cols(sheet)
			row_data = ""
			for row_num in range(3,sheet.nrows):
				for col_num in range(sheet.ncols):
					if col_num in use_cols:
						row_data = row_data + "{0},".format(parse_cell(sheet,row_num, col_num))
				row_data = row_data[:-1] + "\n"
			save_csv(csv_file_name, row_data[:-1])
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

def parse_cell(sheet, row, col):
	tmp_cell_value = sheet.cell_value(row, col)
	tmp_cell_type = sheet.cell_type(row, col)
	if tmp_cell_type == 1:
		tmp_cell_value = tmp_cell_value.replace("\n","</br>")
	elif tmp_cell_type == 2:
		tmp_cell_value = int(tmp_cell_value)
	return tmp_cell_value


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
			if os.path.splitext(file)[1] == ".xlsx":  
				if not file.startswith("~$"):
					excel_to_sheet(file)

def collect_use_cols(sheet):
	use_col_list = []
	for col_num in range(sheet.ncols):
		value = int(sheet.cell_value(USE_COL_NUM,col_num))
		if(value == USE_CLIENT_NUMFLAG or value == USE_ALL_NUMFLAG):
			use_col_list.append(int(col_num))
	return use_col_list

def delete_all_csv(dest_path):
    ls = os.listdir(dest_path)
    for i in ls:
        c_path = os.path.join(dest_path, i)
        if os.path.isdir(c_path):
            del_file(c_path)
        else:
            os.remove(c_path)
    print("clear all csv success! => " + dest_path)

def copy_csv_to_project():
	for root, dirs, files in os.walk( CSV_DIR_PATH):  
		for file in files:  
			if os.path.splitext(file)[1] == ".csv":
				scr_full_path = CSV_DIR_PATH + "/" + file
				dst_full_path = PROJECT_CSV_FULL_PATH + "/" + file
				shutil.copy(scr_full_path,dst_full_path)
	print("copy all csv to project success!")

if __name__ == '__main__':
	delete_all_csv(CSV_DIR_PATH)
	collect_all_excel()
	delete_all_csv(PROJECT_CSV_FULL_PATH)
	copy_csv_to_project()
	print("export excel to csv finish !!!!!!!!!!")
	os.system("pause")