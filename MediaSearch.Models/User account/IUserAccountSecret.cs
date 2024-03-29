﻿namespace MediaSearch.Models;

public interface IUserAccountSecret : IJson<IUserAccountSecret> {

  string Name { get; set; }

  string PasswordHash { get; set; }

  bool MustChangePassword { get; set; }

  IUserToken Token { get; set; }

  void Duplicate(IUserAccountSecret secret);

}
