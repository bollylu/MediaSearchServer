﻿namespace MediaSearch.Database;

public partial interface IMSDatabase {

  /// <summary>
  /// Write a record to the table. If the record exists, it is overwritten
  /// </summary>
  /// <param name="table">The table to write the record to</param>
  /// <param name="record">The record to write</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Write(IMSTable table, IMSRecord record);

  /// <summary>
  /// Write a record to the table. If the record exists, it is overwritten
  /// </summary>
  /// <param name="table">The name of the table to write the record to</param>
  /// <param name="record">The record to write</param>
  /// <returns><see langword="true"/> if ok, <see langword="false"/> otherwise</returns>
  bool Write(string table, IMSRecord record);

  /// <summary>
  /// Read a record from the table
  /// </summary>
  /// <param name="table">The table to read the record from</param>
  /// <param name="key">The key to access the record</param>
  /// <returns>The record already casted to RECORD type or <see langword="null"/> in case of error</returns>
  RECORD Read<RECORD>(IMSTable table, string key) where RECORD : IMSRecord;

  /// <summary>
  /// Read a record from the table
  /// </summary>
  /// <param name="table">The name of the table to read the record from</param>
  /// <param name="key">The key to access the record</param>
  /// <returns>The record already casted to RECORD type or <see langword="null"/> in case of error</returns>
  RECORD Read<RECORD>(string table, string key) where RECORD : IMSRecord;

  /// <summary>
  /// Dump the raw content of a table for debug purpose
  /// </summary>
  /// <param name="table">The table where the record belongs</param>
  /// <param name="key">The ID of the record</param>
  /// <returns>A string describing the raw content</returns>
  string RecordDump(IMSTable table, string key);
}
