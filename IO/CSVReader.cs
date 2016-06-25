using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace mz.betainteractive.utilities.module.IO {
    public class CSVReader {
        private String textcsv = null;
        private FileInfo filecsv = null;
        private byte[] bytescsv = null;
        private Stream streamcsv = null;

        private String DELIMITER = ";";
        private Dictionary<String, int> mapFields = new Dictionary<string, int>();
        private bool hasHeader = true;
        private String currentLine = null;
        private bool reading = false;
        private StreamReader stream;

        public CSVReader(FileInfo file) {
            filecsv = file;
            Start();
        }

        public CSVReader(FileInfo file, bool hasFieldName) {
            filecsv = file;
            hasHeader = hasFieldName;
            Start();
        }

        public CSVReader(FileInfo file, String delimiter){
            filecsv = file;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(FileInfo file, bool hasFieldName, String delimiter) {
            filecsv = file;
            hasHeader = hasFieldName;
            this.DELIMITER = delimiter;            
            Start();
        }

        public CSVReader(Stream fileStream) {
            streamcsv = fileStream;
            Start();
        }

        public CSVReader(Stream fileStream, bool hasFieldName) {            
            streamcsv = fileStream;
            hasHeader = hasFieldName;
            Start();
        }

        public CSVReader(Stream fileStream, String delimiter) {
            streamcsv = fileStream;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(Stream fileStream, bool hasFieldName, String delimiter) {
            streamcsv = fileStream;
            hasHeader = hasFieldName;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(String text) {
            textcsv = text;
            Start();
        }

        public CSVReader(String text, bool hasFieldName)  {
            textcsv = text;
            hasHeader = hasFieldName;
            Start();
        }

        public CSVReader(String text, String delimiter) {
            textcsv = text;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(String text, bool hasFieldName, String delimiter) {
            textcsv = text;
            hasHeader = hasFieldName;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(byte[] bytes) {
            bytescsv = bytes;
            Start();
        }

        public CSVReader(byte[] bytes, bool hasFieldName) {
            bytescsv = bytes;
            hasHeader = hasFieldName;
            Start();
        }

        public CSVReader(byte[] bytes, String delimiter) {
            bytescsv = bytes;
            this.DELIMITER = delimiter;
            Start();
        }

        public CSVReader(byte[] bytes, bool hasFieldName, String delimiter) {
            bytescsv = bytes;
            hasHeader = hasFieldName;
            this.DELIMITER = delimiter;
        }

        private void fillMapFields(String[] fields) {
            mapFields.Clear();

            for (int i = 0; i < fields.Length; i++) {
                mapFields.Add(fields[i], i);
            }
        }

        private void Start() {
            reading = false;
            mapFields.Clear();
            currentLine = null;

            if (stream != null) {
                stream.Close();
                stream = null;
            }

            reading = true;

            if (streamcsv != null) {
                stream = new StreamReader(streamcsv);
            }

            if (filecsv != null) {
                stream = new StreamReader(filecsv.OpenRead());
            }

            if (bytescsv != null) {
                MemoryStream ms = new MemoryStream(bytescsv);
                stream = new StreamReader(ms);
            }

            if (textcsv != null) {
                MemoryStream ms = new MemoryStream(ToByteArray(textcsv));
                stream = new StreamReader(ms);
            }

            ReadFirstLine();
        }

        private byte[] ToByteArray(String text) {
            char[] chrs = text.ToCharArray();
            byte[] bytes = new byte[chrs.Length];

            for (int i = 0; i < bytes.Length; i++) {
                bytes[i] = (byte) chrs[i];
            }

            return bytes;
        }

        public void Close() {
            reading = false;
            mapFields.Clear();
            currentLine = null;
            filecsv = null;

            if (stream != null){
                stream.Close();
                stream = null;
            }
        }

        public bool ReadNextRow() {
            return NextRow();
        }

        private void ReadLine() {

            if (stream == null){
                currentLine = null;
                reading = false;
                return;
            }
       
        }

        private bool NextRow() {

            if (stream == null) {
                currentLine = null;
                reading = false;
                return false;
            }

            reading = true;

            String line = stream.ReadLine();

            if (line == null) {
                reading = false;
                return false;
            }

            reading = !stream.EndOfStream;
            currentLine = line;

            if (stream.EndOfStream && currentLine != null) {
                reading = false;
                return true;
            }

            return reading;
        }

        private void ReadFirstLine() {
            if (hasHeader) {
                try {
                    String line = stream.ReadLine();

                    if (line == null) {
                        reading = false;
                        return;
                    }

                    String[] fields = line.Split(DELIMITER.ToCharArray());
                    fillMapFields(fields);

                    reading = !stream.EndOfStream;

                } catch (Exception ex) {
                    reading = false;
                }

            } else {
                if (stream != null) {
                    reading = !stream.EndOfStream;
                } else {
                    reading = false;
                }
            }

        }

        public int? getLongField(String fieldName) {
            
            String value = getField(fieldName);

            Console.WriteLine("saldo: " + value);
            try {
                int v = Int32.Parse(value);
                return v;
            } catch (Exception ex) {
                return null;
            }
        }

        public int? getLongField(int index) {
            
            String value = getField(index);
            try {
                int v = Int32.Parse(value);
                return v;
            } catch (Exception ex) {
                return null;
            }
        }

        public double? getDoubleField(String fieldName) {
            
            String value = getField(fieldName);
            try {
                double v = Double.Parse(value);
                return v;
            } catch (Exception ex) {
                return null;
            }
        }

        public double? getDoubleField(int index) {
            
            String value = getField(index);
            try {
                double v = Double.Parse(value);
                return v;
            } catch (Exception ex) {
                return null;
            }
        }

        public String getField(String fieldName) {
            
            if (currentLine == null || (reading == false && currentLine == null)) {
                return null;
            }

            int index;
            String value = null;
            String[] values = currentLine.Split(DELIMITER.ToCharArray());

            try {
                index = (int) mapFields[fieldName];
                value = values[index];
            } catch (Exception ex) { }

            return value;
        }

        public String getField(int index) {
            
            if (currentLine == null) {
                return null;
            }

            String value = null;
            String[] values = currentLine.Split(DELIMITER.ToCharArray());

            try {
                value = values[index];
            } catch (Exception ex) { }

            return value;
        }
    }
}
