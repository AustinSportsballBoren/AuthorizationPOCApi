public static class Config
{

  public static string GetOpenFGAModelString()
  {
    string jsonData = @"
{
  ""schema_version"": ""1.1"",
  ""type_definitions"": [
    {
      ""type"": ""user"",
      ""relations"": {},
      ""metadata"": null
    },
    {
      ""type"": ""cabinet"",
      ""relations"": {
        ""member"": {
          ""union"": {
            ""child"": [
              {
                ""this"": {}
              },
              {
                ""computedUserset"": {
                  ""relation"": ""administrator""
                }
              }
            ]
          }
        },
        ""administrator"": {
          ""this"": {}
        }
      },
      ""metadata"": {
        ""relations"": {
          ""member"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          },
          ""administrator"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          }
        }
      }
    },
    {
      ""type"": ""group"",
      ""relations"": {
        ""member"": {
          ""this"": {}
        }
      },
      ""metadata"": {
        ""relations"": {
          ""member"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              }
            ]
          }
        }
      }
    },
    {
      ""type"": ""envelope"",
      ""relations"": {
        ""cabinet"": {
          ""this"": {}
        },
        ""viewer"": {
          ""union"": {
            ""child"": [
              {
                ""difference"": {
                  ""base"": {
                    ""intersection"": {
                      ""child"": [
                        {
                          ""union"": {
                            ""child"": [
                              {
                                ""this"": {}
                              },
                              {
                                ""computedUserset"": {
                                  ""relation"": ""editor""
                                }
                              },
                              {
                                ""computedUserset"": {
                                  ""relation"": ""administrator""
                                }
                              }
                            ]
                          }
                        },
                        {
                          ""tupleToUserset"": {
                            ""computedUserset"": {
                              ""relation"": ""member""
                            },
                            ""tupleset"": {
                              ""relation"": ""cabinet""
                            }
                          }
                        }
                      ]
                    }
                  },
                  ""subtract"": {
                    ""computedUserset"": {
                      ""relation"": ""none""
                    }
                  }
                }
              },
              {
                ""tupleToUserset"": {
                  ""computedUserset"": {
                    ""relation"": ""administrator""
                  },
                  ""tupleset"": {
                    ""relation"": ""cabinet""
                  }
                }
              }
            ]
          }
        },
        ""editor"": {
          ""union"": {
            ""child"": [
              {
                ""difference"": {
                  ""base"": {
                    ""intersection"": {
                      ""child"": [
                        {
                          ""this"": {}
                        },
                        {
                          ""tupleToUserset"": {
                            ""computedUserset"": {
                              ""relation"": ""member""
                            },
                            ""tupleset"": {
                              ""relation"": ""cabinet""
                            }
                          }
                        }
                      ]
                    }
                  },
                  ""subtract"": {
                    ""computedUserset"": {
                      ""relation"": ""none""
                    }
                  }
                }
              },
              {
                ""tupleToUserset"": {
                  ""computedUserset"": {
                    ""relation"": ""administrator""
                  },
                  ""tupleset"": {
                    ""relation"": ""cabinet""
                  }
                }
              }
            ]
          }
        },
        ""sharer"": {
          ""union"": {
            ""child"": [
              {
                ""difference"": {
                  ""base"": {
                    ""intersection"": {
                      ""child"": [
                        {
                          ""union"": {
                            ""child"": [
                              {
                                ""this"": {}
                              },
                              {
                                ""computedUserset"": {
                                  ""relation"": ""administrator""
                                }
                              }
                            ]
                          }
                        },
                        {
                          ""tupleToUserset"": {
                            ""computedUserset"": {
                              ""relation"": ""member""
                            },
                            ""tupleset"": {
                              ""relation"": ""cabinet""
                            }
                          }
                        }
                      ]
                    }
                  },
                  ""subtract"": {
                    ""computedUserset"": {
                      ""relation"": ""none""
                    }
                  }
                }
              },
              {
                ""tupleToUserset"": {
                  ""computedUserset"": {
                    ""relation"": ""administrator""
                  },
                  ""tupleset"": {
                    ""relation"": ""cabinet""
                  }
                }
              }
            ]
          }
        },
        ""administrator"": {
          ""union"": {
            ""child"": [
              {
                ""difference"": {
                  ""base"": {
                    ""intersection"": {
                      ""child"": [
                        {
                          ""this"": {}
                        },
                        {
                          ""tupleToUserset"": {
                            ""computedUserset"": {
                              ""relation"": ""member""
                            },
                            ""tupleset"": {
                              ""relation"": ""cabinet""
                            }
                          }
                        }
                      ]
                    }
                  },
                  ""subtract"": {
                    ""computedUserset"": {
                      ""relation"": ""none""
                    }
                  }
                }
              },
              {
                ""tupleToUserset"": {
                  ""computedUserset"": {
                    ""relation"": ""administrator""
                  },
                  ""tupleset"": {
                    ""relation"": ""cabinet""
                  }
                }
              }
            ]
          }
        },
        ""none"": {
          ""this"": {}
        }
      },
      ""metadata"": {
        ""relations"": {
          ""cabinet"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""cabinet""
              }
            ]
          },
          ""viewer"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          },
          ""editor"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          },
          ""sharer"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          },
          ""administrator"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          },
          ""none"": {
            ""directly_related_user_types"": [
              {
                ""type"": ""user""
              },
              {
                ""type"": ""group""
              },
              {
                ""type"": ""group"",
                ""relation"": ""member""
              }
            ]
          }
        }
      }
    }
  ]
}";
    return jsonData;
  }
}