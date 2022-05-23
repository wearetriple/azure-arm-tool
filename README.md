# azure-arm-tool
Command line tool for manipulating arm templates

The arm tool supports the following commands:
-nest

---The nest command---

  Args: --inputFile <input file>, --outputFile <output file>

  The nest command will output a main arm template by giving different arm templates as input.
  
  !make sure the folder structure for the output file exists.
  
  Example: nest --inputFile <file> --inputFile <file> --outputFile <destination file>
